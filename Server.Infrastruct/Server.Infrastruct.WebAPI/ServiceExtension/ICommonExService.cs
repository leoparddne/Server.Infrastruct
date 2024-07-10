using Server.Domain.Dto.InDto;
using Server.Domain.Dto.InDto.Search;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Model;

namespace Server.Infrastruct.WebAPI.ServiceExtension
{
    /// <summary>
    /// 通用服务 提供基础的增删改查及启用禁用(逻辑删除)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonExService<T> : IBaseService where T : CommonEntity, IEnabled, IDelete, new()
    {
        void Create(T entity);
        void Create(List<T> entity);
        void CreateOrUpdate(List<T> data);
        void CreateOrUpdate(T entity);
        void Delete(List<string> ids);
        T GetByID(string id);
        List<T> GetEnableList();
        PageModel<T> GetEnablePageList(int pageIndex, int pageSize);
        void SetEnabled(SetListEnabledInDto dto);
        void Update(T entity);
        void Update(List<T> entity);

        bool Exist(string id);

        PageModel<T> GetConditionPage(SearchConditionPageInDto dto);
        List<T> GetCondition(List<SearchConditionItem> dto);
    }
}
