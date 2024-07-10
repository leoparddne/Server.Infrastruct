using SqlSugar;
using System.Data;

namespace Server.Infrastruct.WebAPI.Repository
{
    public interface IBaseRepositoryExtension : IDisposable
    {
        ISqlSugarClient sqlSugarClient { get; set; }

        void ChangeDB(string dbID);
        void ChangeDefaultDB(string dbID);
        ISqlSugarClient GetDB(string dbID);

        bool CreateDatabase(string databaseDirectory = null);
        #region Procedure
        DataTable ExecuteProcedure(string procedureName, params SugarParameter[] parameters);

        bool ExecuteProcedure(string procedureName, List<SugarParameter> sugarParameterList, out dynamic model, out string returnMessage);
        #endregion

        #region SQL
        List<T> SqlQuery<T>(string sql, List<SugarParameter> sugarParameterList = null);

        string SqlQuerySingle(string sql, List<SugarParameter> sugarParameterList = null);
        #endregion
    }
}
