using Server.Infrastruct.WebAPI.UnitOfWork;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.Repository.Common
{
    public class CommonRepository<T> : BaseRepositoryExtension where T : class, new()
    {

        protected CommonRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region create
        public IInsertable<T> Add(T entity)
        {
            return sqlSugarClient.Insertable(entity);
        }

        public IInsertable<T> Add(List<T> entity)
        {
            return sqlSugarClient.Insertable(entity);
        }

        #endregion


        #region delete
        public IDeleteable<T> Delete(T entity)
        {
            return sqlSugarClient.Deleteable(entity);
        }

        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IDeleteable<T> Delete(Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Deleteable(condition);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public IDeleteable<T> Delete(dynamic pk)
        {
            return sqlSugarClient.Deleteable(pk);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IDeleteable<T> Delete(List<T> entity)
        {
            return sqlSugarClient.Deleteable(entity);
        }

        #endregion


        #region update
        public IUpdateable<T> Update(T entity)
        {
            return sqlSugarClient.Updateable(entity);
        }

        public IUpdateable<T> Update(List<T> entity)
        {
            return sqlSugarClient.Updateable(entity);
        }
        #endregion

        #region select
        public ISugarQueryable<T> Select(Expression<Func<T, T>> condition)
        {
            return sqlSugarClient.Queryable<T>().Select(condition);
        }


        public T Single(Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Queryable<T>().Single(condition);
        }

        #endregion
    }
}
