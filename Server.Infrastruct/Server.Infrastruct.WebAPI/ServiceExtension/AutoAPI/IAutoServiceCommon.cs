using Server.Domain.Dto.InDto;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Model;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public interface IAutoServiceCommon<T> : IAutoServiceBase<T> where T : CommonEntity, IEnabled, new()
    {
        //public IBaseRepository<T> Repository { get; set; }
        void Create(T entity);
        void Delete(List<string> ids);
        T GetByID(string id);
        List<T> GetEnableList();
        PageModel<T> GetEnablePageList(int pageIndex, int pageSize);
        void SetEnabled(SetListEnabledInDto dto, Expression<Func<T, bool>> whereCondition);
        void Update(List<T> entity);
    }
}
