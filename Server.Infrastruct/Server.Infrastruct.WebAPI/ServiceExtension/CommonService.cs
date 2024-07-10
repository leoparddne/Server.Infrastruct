using Common.Toolkit.Helper;
using Server.Domain.Dto.InDto;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.Authentication;
using Server.Infrastruct.WebAPI.UnitOfWork;

namespace Server.Infrastruct.WebAPI.ServiceExtension
{
    /// <summary>
    /// 通用服务 提供基础的增删改查及启用禁用功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonService<T> : BaseService, ICommonService<T> where T : CommonEntity, ICommonID, IEnabled, new()
    {
        private IBaseRepository<T> repository;
        private IAuthenticationService authenticationService;
        private UserInfoModel userModel { get { return authenticationService.GetUserInfo(); } }

        public CommonService(IBaseRepository<T> repository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService) : base(unitOfWork)
        {
            this.repository = repository;
            this.authenticationService = authenticationService;
        }

        public void Create(T entity)
        {
            //id生成
            entity.Id = GUIDHelper.NewGuid;
            entity.Create(userModel.UserNo);
            repository.Insert(entity);
        }

        public void Delete(List<string> ids)
        {
            repository.DeleteByIdList(ids);
        }

        public void Update(T entity)
        {
            entity.Update(userModel.UserNo);
            repository.Update(entity);
        }

        public void Update(List<T> entity)
        {
            repository.UpdateRange(entity);
        }

        public void SetEnabled(SetListEnabledInDto dto)
        {
            if (dto == null)
            {
                return;
            }

            //根据主键更新
            repository.Update(f => new T { IsEnabled = dto.IsEnabled }, f => dto.Ids.Contains(f.Id));
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByID(string id)
        {
            return repository.QueryById(id);
        }

        /// <summary>
        /// 获取所有启用中的数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetEnableList()
        {
            return repository.QueryListByCondition(f => f.IsEnabled == StatesEnum.Y.ToString());
        }

        /// <summary>
        /// 获取所有启用中的数据(分页)
        /// </summary>
        /// <returns></returns>
        public PageModel<T> GetEnablePageList(int pageIndex, int pageSize)
        {
            return repository.QueryPageList(f => f.IsEnabled == StatesEnum.Y.ToString(), f => f.UpdateTime, pageIndex, pageSize, SqlSugar.OrderByType.Desc);
        }
    }
}
