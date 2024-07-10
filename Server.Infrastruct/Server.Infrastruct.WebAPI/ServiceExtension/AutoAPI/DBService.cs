using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.UnitOfWork;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public class DBService<T> : BaseService, IDBService<T> where T : class
    {
        protected readonly IBaseRepository<T> repository;

        public DBService(IUnitOfWork unitOfWork, IBaseRepository<T> repository) : base(unitOfWork)
        {
            this.repository = repository;
        }

        public void Create(T entity)
        {
            repository.Insert(entity);
        }

        public void Delete(List<string> ids)
        {
            repository.DeleteByIdList(ids);
        }

        public void Update(List<T> entity)
        {
            repository.UpdateRange(entity);
        }

        public List<T> Get(Expression<Func<T, bool>> whereCondition,
                                     Expression<Func<T, object>> orderByCondition = null,
                                     OrderByType orderByType = OrderByType.Asc)
        {
            return repository.QueryListByCondition(whereCondition, orderByCondition, orderByType);
        }


        public PageModel<T> Get(Expression<Func<T, bool>> whereCondition,
                                   Expression<Func<T, object>> orderByCondition,
                                   int pageIndex = 1,
                                   int pageSize = 20,
                                   OrderByType orderByType = OrderByType.Asc)
        {
            return repository.QueryPageList(whereCondition, orderByCondition, pageIndex, pageSize, orderByType);
        }
    }
}
