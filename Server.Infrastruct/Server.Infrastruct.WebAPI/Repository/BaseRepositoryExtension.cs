using Common.Toolkit.Helper;
using Microsoft.IdentityModel.Tokens;
using Server.Infrastruct.WebAPI.Model;
using Server.Infrastruct.WebAPI.Model.Constant;
using Server.Infrastruct.WebAPI.UnitOfWork;
using SqlSugar;
using System.Data;
using System.Dynamic;

namespace Server.Infrastruct.WebAPI.Repository
{
    public class BaseRepositoryExtension : IBaseRepositoryExtension, IDisposable
    {
        public ISqlSugarClient sqlSugarClient { get; set; }

        //public SqlSugarScope sqlSugarClient { get; set; }

        private IUnitOfWork unitOfWork;

        public BaseRepositoryExtension(IUnitOfWork unitOfWork)
        {
            sqlSugarClient = unitOfWork.GetInstance();
            this.unitOfWork = unitOfWork;
        }

        public ISqlSugarClient GetDB(string dbID)
        {
            //SqlSugarProvider scopeProvider = sqlSugarClient.GetConnection(dbID);
            SqlSugarScopeProvider scopeProvider = unitOfWork.GetInstance().GetConnectionScope(dbID);
            if (scopeProvider != null)
            {
                if (scopeProvider is ISqlSugarClient sugarScope)
                {
                    return sugarScope;
                }
            }

            return null;
        }

        /// <summary>
        /// 注意此方法调用需要在同一个上下文中,否则切换的是不同上下文的数据库
        /// 例：如在service内部直接调用ChangeDB,然后通过仓储查询,此时service内部和仓储使用的上下文不同将查询不同数据
        /// 此时应该调整为通过仓储调用ChangeDB,然后在通过仓储查询数据,或者均通过service切换及查询
        /// ！！！一般情况下都应该通过仓储操作,除非是做特殊查询如手动执行sql这一类场景可以直接通过service查询数据库
        /// </summary>
        /// <param name="dbID"></param>
        public void ChangeDB(string dbID)
        {
            var db = GetDB(dbID);
            ExceptionHelper.CheckException(db == null, $"dbid:{dbID} can not fetch");
            sqlSugarClient = db;
        }

        /// <summary>
        /// 切换默认数据库 - 不推荐使用
        /// </summary>
        /// <param name="dbID"></param>
        [Obsolete]
        public void ChangeDefaultDB(string dbID)
        {
            unitOfWork.GetInstance().ChangeDatabase(dbID);
        }


        /// <summary>
        /// 转换成小写
        /// </summary>
        /// <param name="sugarParameterList"></param>
        /// <returns></returns>
        private List<SugarParameter> Copy2LowerList(List<SugarParameter> sugarParameterList)
        {

            if (sugarParameterList.IsNullOrEmpty() || !DBConfigSingleton.GetConfig().SqlAutoToLower)
            {
                return sugarParameterList;
            }

            List<SugarParameter> copy = new List<SugarParameter>();

            foreach (var item in sugarParameterList)
            {
                var para = new SugarParameter(item.ParameterName.ToLower(), item.Value, item.Direction == ParameterDirection.Output);
                para.DbType = item.DbType;
                copy.Add(para);
            }

            return copy;
        }

        /// <summary>
        /// 还原参数
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="returnParameter"></param>
        /// <returns></returns>
        private List<SugarParameter> ReverseParameter(List<SugarParameter> origin, List<SugarParameter> returnParameter)
        {
            if (origin.IsNullOrEmpty() || !DBConfigSingleton.GetConfig().SqlAutoToLower)
            {
                return origin;
            }

            foreach (var item in origin)
            {
                var parameter = returnParameter.FirstOrDefault(f => f.ParameterName.ToUpper() == item.ParameterName.ToUpper());
                if (parameter == null)
                {
                    continue;
                }
                item.Value = parameter.Value;
            }

            return origin;
        }


        #region Procedure
        public DataTable ExecuteProcedure(string procedureName, params SugarParameter[] parameters)
        {
            var oldPara = parameters.ToList();
            var para = Copy2LowerList(oldPara);

            DataTable dt = sqlSugarClient.Ado.UseStoredProcedure().GetDataTable(procedureName, para);
            var origin = ReverseParameter(oldPara, para);

            var res = origin.FirstOrDefault(f => f.ParameterName.ToUpper() == "O_RES");
            if (res != null)
            {
                var result = res.Value.ToString() == AppConstant.PROCEDURE_SUCCESS;
                if (!result)
                {
                    throw new Exception($"{procedureName}:{res.Value}");
                }
            }

            return dt;
        }

        public bool ExecuteProcedure(string procedureName, List<SugarParameter> sugarParameterList, out dynamic model, out string returnMessage)
        {
            var para = Copy2LowerList(sugarParameterList);
            model = null;
            bool result = false;
            returnMessage = string.Empty;
            DataTable dt = sqlSugarClient.Ado.UseStoredProcedure().GetDataTable(procedureName, para);

            var origin = ReverseParameter(sugarParameterList, para);

            if (origin is not null && origin.Count > 0)
            {
                List<SugarParameter> outputList = origin.Where(t => t.Direction == ParameterDirection.Output).ToList();
                if (outputList is not null && outputList.Count > 0)
                {
                    model = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)model;

                    foreach (SugarParameter output in outputList)
                    {
                        var isDBNull = output.Value.GetType() == typeof(DBNull);
                        object objValue = isDBNull ? null : output.Value;
                        string attributeName = AssemblyAttributeName(output.ParameterName);
                        if (attributeName.Equals("Res"))
                        {
                            result = output.Value.ToString() == AppConstant.PROCEDURE_SUCCESS;
                            if (!result)
                            {
                                returnMessage = $"{procedureName}:{objValue}";
                                throw new Exception(returnMessage);
                            }
                        }
                        else
                        {
                            dictionary[attributeName] = objValue;
                        }
                    }
                }
            }

            return result;
        }

        private string AssemblyAttributeName(string columnName)
        {
            string attributeName = string.Empty;
            if (columnName.Contains("_"))
            {
                string[] attributeNameArray = columnName.Split("_");
                for (int i = 1; i < attributeNameArray.Length; i++)
                {
                    string beforeAttributeName = attributeNameArray[i].ToLower();
                    if (beforeAttributeName.Equals("upn"))
                    {
                        attributeName = "UPN";
                    }
                    else
                    {
                        attributeName = $"{attributeName}{beforeAttributeName.Substring(0, 1).ToUpper()}{beforeAttributeName.Substring(1)}";
                    }
                }
                return attributeName;
            }
            else
            {
                return columnName;
            }
        }
        #endregion

        #region SQL
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sugarParameterList"></param>
        /// <returns></returns>
        public List<T> SqlQuery<T>(string sql, List<SugarParameter> sugarParameterList = null)
        {
            return sqlSugarClient.Ado.SqlQuery<T>(sql, sugarParameterList);
        }

        /// <summary>
        /// 执行sql语句,返回单一数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sugarParameterList"></param>
        /// <returns></returns>
        public string SqlQuerySingle(string sql, List<SugarParameter> sugarParameterList = null)
        {
            return sqlSugarClient.Ado.SqlQuerySingle<string>(sql, sugarParameterList);
        }

        #endregion


        public void Dispose()
        {
            sqlSugarClient.Dispose();
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseDirectory"></param>
        /// <returns></returns>
        public bool CreateDatabase(string databaseDirectory = null)
        {
            return sqlSugarClient.DbMaintenance.CreateDatabase();
        }
    }
}
