using Common.Toolkit.Extention;
using Common.Toolkit.Helper;
using Server.Infrastruct.Extension.SqlSugar;
using Server.Infrastruct.Model.DBConfig;
using SqlSugar;
using System.Text;

namespace Server.Infrastruct.WebAPI.CollectionExtension
{
    /// <summary>
    /// SqlSuger注册
    /// </summary>
    public static class SqlSugarCollectionExtension
    {
        public static SqlSugarScope sqlSugarClient;


        public static void AddSqlSugar(this IServiceCollection services, Action<string> logExecuting = null, Action<string> logExecuted = null)
        {
            var config = DBConfigSingleton.GetConfig();
            var connectionConfig = SqlSugerEx.InjectConnectionStr(config);

            sqlSugarClient = new SqlSugarScope(connectionConfig);
            InjectSugar(services, logExecuting, logExecuted);

            //sqlSugarClient.Open();
        }

        /// <summary>
        /// 多数据库支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logExecuting"></param>
        /// <param name="logExecuted"></param>
        public static void AddSqlSugarMuti(this IServiceCollection services, Action<string> logExecuting = null, Action<string> logExecuted = null)
        {
            var configList = DBConfigMutiSingleton.GetConfig();
            if (!configList.IsNullOrEmpty())
            {
                List<ConnectionConfig> connectionConfigs = new List<ConnectionConfig>();
                foreach (var config in configList)
                {
                    var connectionConfig = SqlSugerEx.InjectConnectionStr(config);
                    if (connectionConfig != null)
                    {
                        connectionConfigs.Add(connectionConfig);
                    }
                }

                sqlSugarClient = new SqlSugarScope(connectionConfigs);
            }

            InjectSugar(services, logExecuting, logExecuted);

            //sqlSugarClient.Open();
        }

        private static void InjectSugar(IServiceCollection services, Action<string> logExecuting, Action<string> logExecuted)
        {
            services.AddSingleton<ISqlSugarClient>(s =>
            {
                sqlSugarClient.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //string a = sql;
                    logExecuting?.Invoke(sql);
                    //Console.WriteLine(sql);
                };

                //TODO - 慢查询
                sqlSugarClient.Aop.OnLogExecuted = (sql, pars) =>
                {
                    logExecuted?.Invoke(sql);

                    //执行时间超过3秒
                    if (sqlSugarClient.Ado.SqlExecutionTime.TotalSeconds > 1)
                    {
                        if (sqlSugarClient.Ado.SqlStackTrace == null)
                        {
                            return;
                        }
                        ////代码CS文件名
                        //string fileName = sqlSugarClient.Ado.SqlStackTrace.FirstFileName;
                        ////代码行数
                        //int fileLine = sqlSugarClient.Ado.SqlStackTrace.FirstLine;
                        ////方法名
                        //string FirstMethodName = sqlSugarClient.Ado.SqlStackTrace.FirstMethodName;
                        ////db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息
                        StringBuilder trace = new();
                        foreach (StackTraceInfoItem item in sqlSugarClient.Ado.SqlStackTrace.MyStackTraceList)
                        {
                            trace.Append($"{item.FileName}-{item.Line}-{item.MethodName}");
                        }

                        LogHelper.WriteLog("SlowLog.txt", new string[] { "执行时长{sqlSugarClient.Ado.SqlExecutionTime.TotalSeconds}:{sql}", trace.ToString() });
                    }
                };

                return sqlSugarClient;
            });
        }
    }
}
