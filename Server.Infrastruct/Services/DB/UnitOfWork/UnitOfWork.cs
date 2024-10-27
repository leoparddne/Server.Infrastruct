using SqlSugar;
using System.Data;

namespace Server.Infrastruct.Services.DB.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISqlSugarClient sqlSugarClient;

        public UnitOfWork(ISqlSugarClient SqlSugarClient)
        {
            sqlSugarClient = SqlSugarClient;
        }

        #region GetInstance
        /// <summary>
        /// 获取Sqlsugar连接
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetInstance()
        {
            return sqlSugarClient as SqlSugarScope;
        }
        #endregion

        #region BeginTran
        /// <summary>
        /// 事务开始
        /// </summary>
        public void BeginTran(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            GetInstance().Ado.BeginTran(isolationLevel);
        }
        #endregion

        #region CommitTran
        /// <summary>
        /// 事务提交
        /// </summary>
        public void CommitTran()
        {
            GetInstance().Ado.CommitTran();
        }
        #endregion

        #region RollbackTran
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTran()
        {
            GetInstance().Ado.RollbackTran();
        }
        #endregion

        #region 执行失败自动回滚
        public void AutoTran(Action action, Action<Exception> exceptionAction = null)
        {
            var result = GetInstance().Ado.UseTran(() =>
            {
                action?.Invoke();
                //CommitTran();

            }, exceptionAction);

            //if (!result.IsSuccess)
            //{
            //    RollbackTran();
            //}
        }
        #endregion
    }
}
