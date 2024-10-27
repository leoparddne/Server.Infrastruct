using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Server.DBEx.Dapper.Connection.Factory
{
    public class MySqlConnectionFactory : IConnectionFactoryBase
    {
        public DbConnection GetConnection(string connectionStr)
        {
            return new MySqlConnection(connectionStr);
        }
    }
}
