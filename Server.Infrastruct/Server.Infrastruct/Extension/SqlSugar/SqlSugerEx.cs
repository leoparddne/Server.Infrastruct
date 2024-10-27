using Server.Infrastruct.Model.DBConfig;
using SqlSugar;

namespace Server.Infrastruct.Extension.SqlSugar
{
    public static class SqlSugerEx
    {

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOutput"></param>
        public static void AddSugarParameter(this List<SugarParameter> parameters, string name, object value, bool isOutput = false)
        {
            if (isOutput == false && value != null)
            {
                if (value.GetType() == typeof(int))
                {
                    parameters.Add(new SugarParameter(name, value, System.Data.DbType.Int32));
                    return;
                }
                if (value.GetType() == typeof(double))
                {
                    parameters.Add(new SugarParameter(name, value, System.Data.DbType.Double));
                    return;
                }
            }
            parameters.Add(new SugarParameter(name, value, isOutput));
        }


        /// <summary>
        /// 根据配置获取配置对应的数据库类型
        /// 参数为null时通过单例配置获取，多数据库时需要手动传入配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DbType GetDBType(DBConfigBase? config = null)
        {
            if (config == null)
            {
                config = DBConfigSingleton.GetConfig();
            }

            return (DbType)Enum.Parse(typeof(DbType), config.ConnectionType);
        }



        public static ConnectionConfig InjectConnectionStr(DBConfigBase config)
        {
            //var config = DBConfigSingleton.GetConfig();
            var databaseType = SqlSugerEx.GetDBType(config);

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
    }
}
