using Microsoft.AspNetCore.Mvc;
using Server.Infrastruct.Model.Dto.InDto;
using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.DB;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Base;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.APIService
{
    /// <summary>
    /// 基础api service - 不直接注入而是继承此类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIService<T> : IAutoService where T : CommonEntity, ICommonID, IEnabled, new()
    {
        IEnableService<T> autoService;
        public APIService(IBaseRepository<T> repository, IEnableService<T> autoService)
        {
            this.autoService = autoService;
        }

        [HttpPost]
        public void Create(T entity)
        {
            autoService.Create(entity);
        }

        [HttpPost]
        public void Delete(List<string> ids)
        {
            autoService.Delete(ids);
        }

        [HttpPost]
        public void Update(List<T> entity)
        {
            autoService.Update(entity);
        }

        [HttpPost]
        public void SetEnabled(SetListEnabledInDto dto)
        {
            autoService.SetEnabled(dto);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public T GetByID(string id)
        {
            return autoService.GetByID(id);
        }

        /// <summary>
        /// 获取所有启用中的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<T> GetEnableList()
        {
            return autoService.GetEnableList();
        }

        /// <summary>
        /// 获取所有启用中的数据(分页)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PageModel<T> GetEnablePageList(int pageIndex, int pageSize)
        {
            return autoService.GetEnablePageList(pageIndex, pageSize);
        }
    }
}
