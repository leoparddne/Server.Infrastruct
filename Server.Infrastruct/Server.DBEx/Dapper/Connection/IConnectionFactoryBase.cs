using System.Data.Common;

namespace Server.DBEx.Dapper.Connection
{
    public interface IConnectionFactoryBase
    {
        DbConnection GetConnection(string connectionStr);
    }
}