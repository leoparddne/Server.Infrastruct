using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Server.DBEx.Dapper.Connection.Factory
{
    public class SQLServerConnectionFactory : IConnectionFactoryBase
    {
        public DbConnection GetConnection(string connectionStr)
        {
            return new SqlConnection(connectionStr);
        }
    }
}
