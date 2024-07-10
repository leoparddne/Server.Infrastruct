using Server.Domain.Dto.InDto;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Function;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public class AutoServiceCommon<T> : AutoServiceBase<T>, IAutoCondition, IAutoCreate, IAutoDelete, IAutoSelect, IAutoUpdate, IAutoServiceCommon<T> where T : CommonEntity, IEnabled, new()
    {
        //public IBaseRepository<T> Repository { get; set; }

        public AutoServiceCommon(IBaseRepository<T> repository) : base(repository)
        {
            //this.Repository = repository;
        }

        public void Create(T entity)
        {
            Repository.Insert(entity);
        }

        public void Delete(List<string> ids)
        {
            Repository.DeleteByIdList(ids);
        }

        public void Update(List<T> entity)
        {
            //foreach (var item in entity)
            //{
            //    item.Update(userModel.UserNo);
            //}
            Repository.UpdateRange(entity);
        }

        public void SetEnabled(SetListEnabledInDto dto, Expression<Func<T, bool>> whereCondition)
        {
            if (dto == null)
            {
                return;
            }

            //根据主键更新
            Repository.Update(f => new T { IsEnabled = dto.IsEnabled }, whereCondition);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByID(string id)
        {
            return Repository.QueryById(id);
        }

        /// <summary>
        /// 获取所有启用中的数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetEnableList()
        {
            return Repository.QueryListByCondition(f => f.IsEnabled == StatesEnum.Y.ToString());
        }

        /// <summary>
        /// 获取所有启用中的数据(分页)
        /// </summary>
        /// <returns></returns>
        public PageModel<T> GetEnablePageList(int pageIndex, int pageSize)
        {
            return Repository.QueryPageList(f => f.IsEnabled == StatesEnum.Y.ToString(), f => f.UpdateTime, pageIndex, pageSize, SqlSugar.OrderByType.Desc);
        }
    }
}
