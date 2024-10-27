using Server.Infrastruct.Model.Dto.InDto;
using Server.Infrastruct.Model.Dto.InDto.Search;
using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Services.DB.Base;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.Services.DB
{
    public interface IAutoService<T> : IAutoServiceBase<T> where T : CommonEntity, new()
    {
        //public IBaseRepository<T> Repository { get; set; }
        void Create(T entity);
        void Delete(List<string> ids);
        bool Exist(string id);
        List<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>>? orderByCondition = null, OrderByType orderByType = OrderByType.Asc);
        PageModel<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc);
        T GetByID(string id);
        List<T> GetCondition(List<SearchConditionItem> dto);
        PageModel<T> GetConditionPage(SearchConditionPageInDto dto);
        void Update(List<T> entity);
    }
}
