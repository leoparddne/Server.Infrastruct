using Server.DBEx.Dapper.Connection.Factory;
using Server.DBEx.Model.Enums;
using System.Data.Common;

namespace Server.DBEx.Dapper.Connection
{
    public static class DapperConnectionHelper
    {
        public static DbConnection GetConnection(SQLSugarDBTypeEnum connectType, string connectionString)
        {
            IConnectionFactoryBase factoryBase = null;


            switch (connectType)
            {
                case SQLSugarDBTypeEnum.MySql:
                    break;
                case SQLSugarDBTypeEnum.SqlServer:
                    factoryBase = new SQLServerConnectionFactory();
                    break;
                case SQLSugarDBTypeEnum.Sqlite:
                    factoryBase = new SqliteConnectionFactory();
                    break;
                case SQLSugarDBTypeEnum.Oracle:
                    factoryBase = new OracleConnectionFactory();
                    break;
                case SQLSugarDBTypeEnum.PostgreSQL:
                    factoryBase = new PGSqlConnectionFactory();
                    break;
                case SQLSugarDBTypeEnum.Dm:
                    break;
                case SQLSugarDBTypeEnum.Kdbndp:
                    break;
                case SQLSugarDBTypeEnum.Oscar:
                    break;
                case SQLSugarDBTypeEnum.MySqlConnector:
                    break;
                case SQLSugarDBTypeEnum.Access:
                    break;
                case SQLSugarDBTypeEnum.OpenGauss:
                    break;
                case SQLSugarDBTypeEnum.QuestDB:
                    break;
                case SQLSugarDBTypeEnum.HG:
                    break;
                case SQLSugarDBTypeEnum.ClickHouse:
                    break;
                case SQLSugarDBTypeEnum.GBase:
                    break;
                case SQLSugarDBTypeEnum.Odbc:
                    break;
                case SQLSugarDBTypeEnum.Custom:
                    break;
                default:
                    break;
            }

            if (factoryBase == null)
            {
                throw new NotImplementedException();
            }

            return factoryBase.GetConnection(connectionString);
        }
    }
}
