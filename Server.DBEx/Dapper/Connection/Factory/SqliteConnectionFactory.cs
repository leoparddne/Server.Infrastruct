using System.Data.Common;
using System.Data.SQLite;

namespace Server.DBEx.Dapper.Connection.Factory
{
    public class SqliteConnectionFactory : IConnectionFactoryBase
    {
        public DbConnection GetConnection(string connectionStr)
        {
            return new SQLiteConnection(connectionStr);
        }
    }
}
