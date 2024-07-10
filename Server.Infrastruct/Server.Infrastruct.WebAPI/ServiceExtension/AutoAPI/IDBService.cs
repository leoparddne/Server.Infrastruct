using Server.Domain.Models.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public interface IDBService<T>
    {
        void Create(T entity);
        void Delete(List<string> ids);
        List<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);
        PageModel<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc);
        void Update(List<T> entity);
    }
}
