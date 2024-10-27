using Npgsql;
using System.Data.Common;

namespace Server.DBEx.Dapper.Connection.Factory
{
    public class PGSqlConnectionFactory : IConnectionFactoryBase
    {
        public DbConnection GetConnection(string connectionStr)
        {
            return new NpgsqlConnection(connectionStr);
        }
    }
}
