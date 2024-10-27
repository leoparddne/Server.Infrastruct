using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Server.DBEx.Dapper.Connection.Factory
{
    public class OracleConnectionFactory : IConnectionFactoryBase
    {
        public DbConnection GetConnection(string connectionStr)
        {
            return new OracleConnection(connectionStr);
        }
    }
}
