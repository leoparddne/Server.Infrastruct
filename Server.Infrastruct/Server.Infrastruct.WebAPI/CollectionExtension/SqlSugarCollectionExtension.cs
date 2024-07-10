using Common.Toolkit.Extention;
using Common.Toolkit.Helper;
using Server.Infrastruct.WebAPI.Model;
using SqlSugar;
using System.Text;

namespace Server.Infrastruct.WebAPI.CollectionExtension
{
    /// <summary>
    /// SqlSuger注册
    /// </summary>
    public static class SqlSugarCollectionExtension
    {
        //private static DBConfigSingleton config;
        //private static DbType databaseType;
        //private static List<SqlFuncExternal> sqlFuncList;
        private static SqlSugarScope sqlSugarClient;


        static ConnectionConfig InjectConnectionStr(DBConfigBase config)
        {
            //var config = DBConfigSingleton.GetConfig();
            var databaseType = GetDBType(config);

            var sqlFuncList = new List<SqlFuncExternal>();

            sqlFuncList.Add(new SqlFuncExternal()
            {
                UniqueMethodName = config.ConnectionType + "NVLNull",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if (dbType == databaseType)
                    {
                        return $"nvl({expInfo.Args[0].MemberName},' ')";
                    }
                    else
                    {
                        throw new Exception("Unrealized");
                    }
                }
            });


            sqlFuncList.Add(new SqlFuncExternal()
            {
                UniqueMethodName = config.ConnectionType + "Decode",
                MethodValue = (expInfo, dbType, expContext) =>
                {
                    if (dbType == databaseType)
                    {
                        return $"decode({expInfo.Args[0].MemberName},null,{expInfo.Args[1].MemberName},{expInfo.Args[0].MemberName})";
                    }
                    else
                    {
                        throw new Exception("Unrealized");
                    }
                }
            });
            return InitSugarConnection(config, databaseType, sqlFuncList);



            //sqlSugarClient.Aop.OnError = (exp) =>
            //{
            //    //exp.sql 错误SQL  
            //};


            //sqlSugarClient.Aop.OnExecutingChangeSql = (sql, pars) =>
            //{
            //    //修改SQL和参数名为全小写
            //    if (config.SqlAutoToLower)
            //    {
            //        sql = sql.ToLower();
            //        if (pars != null)
            //        {
            //            foreach (SugarParameter p in pars)
            //            {
            //                p.ParameterName = p.ParameterName.ToLower();
            //            }
            //        }

            //        foreach (var item in pars ?? default)
            //        {
            //            if (item.Value is string strValue)
            //            {
            //                item.Value = strValue.ToLower();
            //            }
            //        }
            //    }

            //    return new KeyValuePair<string, SugarParameter[]>(sql, pars);
            //};
        }

        public static ConnectionConfig InitSugarConnection(DBConfigBase config, DbType databaseType, List<SqlFuncExternal> sqlFuncList)
        {
            var connectionConfig = new ConnectionConfig()
            {
                ConfigId = config.ConfigID,
                ConnectionString = config.ConnectionString,
                DbType = databaseType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    SqlFuncServices = sqlFuncList
                },
                MoreSettings = new ConnMoreSettings()
                {
                    //PgSqlIsAutoToLower = config.SqlAutoToLower
                    //PgSql数据库存在大写字段的 ，需要把这个设为false ，并且实体和字段名称要一样
                },

            };

            return connectionConfig;
        }

        public static void AddSqlSugar(this IServiceCollection services, Action<string> logExecuting = null, Action<string> logExecuted = null)
        {
            var config = DBConfigSingleton.GetConfig();
            var connectionConfig = InjectConnectionStr(config);

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
                    var connectionConfig = InjectConnectionStr(config);
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

        /// <summary>
        /// 参数为null时通过单例配置获取，多数据库时需要手动传入配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DbType GetDBType(DBConfigBase config = null)
        {
            if (config == null)
            {
                config = DBConfigSingleton.GetConfig();
            }

            return (DbType)Enum.Parse(typeof(DbType), config.ConnectionType);
        }
    }
}
