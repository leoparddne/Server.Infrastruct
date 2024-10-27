using Dapper;
using Dapper.Contrib.Extensions;
using Server.DBEx.Dapper.Connection;
using Server.DBEx.Model.Enums;
using System.Data;
using System.Data.Common;


namespace Server.DBEx.DBEx
{
    public class DapperHelper
    {
        //数据库连接字符串
        private string connectionString;

        //数据库连接类型
        private SQLSugarDBTypeEnum commonDBType;



        public DapperHelper(SQLSugarDBTypeEnum connectType, string connectionString)
        {
            this.connectionString = connectionString;
            commonDBType = connectType;
        }


        //private  readonly string connectionString = "Server=127.0.0.1;Database=shuigong;Uid=root;Pwd=123456;";

        public DbConnection GetDbConnection()
        {
            DbConnection connection = DapperConnectionHelper.GetConnection(commonDBType, connectionString);//new OracleConnection(connectionString);
            //connection.Open();
            return connection;
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public List<T> Query<T>(string sql, object param)
        {
            List<T> result = null;
            using (IDbConnection con = GetDbConnection())
            {
                result = con.Query<T>(sql, param).ToList();
            }

            return result;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.Query(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QueryFirst<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QueryFirst(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QueryFirst(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QueryFirstOrDefault(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QueryFirstOrDefault(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QuerySingle<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QuerySingle(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QuerySingle(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QuerySingleOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic QuerySingleOrDefault(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.QuerySingleOrDefault(sql, param);
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.Execute(sql, param);
            }
        }

        /// <summary>
        /// 根据实体类插入表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            using (DbConnection conn = GetDbConnection())
            {
                return await conn.InsertAsync(model, transaction, commandTimeout);
            }
        }

        /// <summary>
        /// 根据实体类更新表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            using (DbConnection conn = GetDbConnection())
            {
                bool b = await conn.UpdateAsync(model, transaction, commandTimeout);
                return b;
            }
        }


        /// <summary>
        /// 根据实体类删除表（实体类需要加标记处理）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            using (DbConnection conn = GetDbConnection())
            {
                bool b = await conn.DeleteAsync(model, transaction, commandTimeout);
                return b;
            }
        }

        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.ExecuteReader(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.ExecuteScalar(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalarFor<T>(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic ExecuteScalarFor(string sql, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                return con.ExecuteScalar(sql, param);
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> ExecutePro<T>(string proc, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                List<T> list = con.Query<T>(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
        }
        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<dynamic> ExecutePro(string proc, object param)
        {
            using (IDbConnection con = GetDbConnection())
            {
                List<dynamic> list = con.Query(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
        }

        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <param name="param">param</param>
        /// <returns></returns>
        public int ExecuteTransaction<T>(string[] sqlarr)
        {
            using (IDbConnection con = GetDbConnection())
            {
                con.Open();
                using (IDbTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (string sql in sqlarr)
                        {
                            result += con.Execute(sql, null, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                con.Close();
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="Key">多条SQL</param>
        /// <param name="Value">param</param>
        /// <returns></returns>
        public int ExecuteTransaction<T>(Dictionary<string, object> dic)
        {
            using (IDbConnection con = GetDbConnection())
            {
                con.Open();
                using (IDbTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (KeyValuePair<string, object> sql in dic)
                        {
                            result += con.Execute(sql.Key, sql.Value, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
                con.Close();
            }
        }
    }

}
