using Server.Domain.Dto.InDto;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Model;

namespace Server.Infrastruct.WebAPI.ServiceExtension
{
    /// <summary>
    /// 通用服务 提供基础的增删改查及启用禁用功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonService<T> : IBaseService where T : CommonEntity, IEnabled, new()
    {
        void Create(T entity);
        void Delete(List<string> ids);
        T GetByID(string id);
        List<T> GetEnableList();
        PageModel<T> GetEnablePageList(int pageIndex, int pageSize);
        void SetEnabled(SetListEnabledInDto dto);
        void Update(T entity);
        void Update(List<T> entity);
    }
}
