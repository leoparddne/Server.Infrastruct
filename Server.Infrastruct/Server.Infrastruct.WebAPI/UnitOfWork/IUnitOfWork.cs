using SqlSugar;
using System.Data;

namespace Server.Infrastruct.WebAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region GetInstance
        /// <summary>
        /// 获取Sqlsugar连接
        /// </summary>
        /// <returns></returns>
        SqlSugarScope GetInstance();
        #endregion

        #region BeginTran
        /// <summary>
        /// 事务开始
        /// </summary>
        void BeginTran(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        #endregion

        #region CommitTran
        /// <summary>
        /// 事务提交
        /// </summary>
        void CommitTran();
        #endregion

        #region RollbackTran
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollbackTran();
        #endregion

        #region 自动回滚事务
        public void AutoTran(Action action, Action<Exception> exceptionAction = null);

        #endregion
    }
}
