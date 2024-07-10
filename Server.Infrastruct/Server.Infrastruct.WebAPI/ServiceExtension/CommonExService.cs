using Common.Toolkit.Helper;
using Server.Domain.Dto.InDto;
using Server.Domain.Dto.InDto.Search;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Enums.Search;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.Authentication;
using Server.Infrastruct.WebAPI.UnitOfWork;

namespace Server.Infrastruct.WebAPI.ServiceExtension
{
    /// <summary>
    /// 通用服务 提供基础的增删改查及启用禁用功能(逻辑删除)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonExService<T> : BaseService, ICommonExService<T> where T : CommonEntity, ICommonID, IEnabled, IDelete, new()
    {
        private IBaseRepository<T> repository;
        private IAuthenticationService authenticationService;
        private UserInfoModel userModel { get { return authenticationService.GetUserInfo(); } }

        public CommonExService(IBaseRepository<T> repository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService) : base(unitOfWork)
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

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(List<string> ids)
        {
            DateTime dateTime = DateTime.Now;
            repository.Update(l => new T { IsDelete = 1, UpdateTime = dateTime, UpdateUser = userModel.UserNo }, l => ids.Contains(l.Id));
        }

        public void Update(T entity)
        {
            entity.Update(userModel.UserNo);
            repository.Update(entity);
        }

        public void Update(List<T> entity)
        {
            //id生成
            foreach (var item in entity)
            {
                item.Update(authenticationService.GetUserNo());
            }
            repository.UpdateRange(entity);
        }

        public void Create(List<T> entity)
        {
            //id生成
            foreach (var item in entity)
            {
                item.Id = GUIDHelper.NewGuid;
                item.Create(authenticationService.GetUserNo());
            }

            repository.InsertRange(entity);
        }

        public void CreateOrUpdate(T entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                Create(entity);
            }
            else
            {
                Update(entity);
            }
        }

        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <param name="data"></param>
        public void CreateOrUpdate(List<T> data)
        {
            var create = data.Where(f => string.IsNullOrWhiteSpace(f.Id)).ToList();
            var update = data.Where(f => !string.IsNullOrWhiteSpace(f.Id)).ToList();

            if (create?.Any() ?? false)
            {
                Create(create);
            }
            if (update?.Any() ?? false)
            {
                Update(update);
            }
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
            var data = repository.QueryById(id);
            return data.IsDelete == 0 ? data : null;
        }

        /// <summary>
        /// 获取所有启用中的数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetEnableList()
        {
            return repository.QueryListByCondition(f => f.IsEnabled == StatesEnum.Y.ToString() && f.IsDelete == 0);
        }

        /// <summary>
        /// 获取所有启用中的数据(分页)
        /// </summary>
        /// <returns></returns>
        public PageModel<T> GetEnablePageList(int pageIndex, int pageSize)
        {
            return repository.QueryPageList(f => f.IsEnabled == StatesEnum.Y.ToString() && f.IsDelete == 0, f => f.UpdateTime, pageIndex, pageSize, SqlSugar.OrderByType.Desc);
        }

        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(string id)
        {
            return repository.Exists(l => l.Id == id && l.IsDelete == 0);
        }

        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PageModel<T> GetConditionPage(SearchConditionPageInDto dto)
        {
            dto.Conditions.Add(new SearchConditionItem
            {
                FieldName = "IsDelete",
                ConditionType = SearchTypeEnum.Equal,
                Value = "0",
                WhereType = SearchWhereTypeEnum.And
            });
            PageModel<T> data = repository.GetPageModelByCondition(dto.Conditions, null, dto.PageIndex, dto.PageSize, false);
            return data;
        }


        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<T> GetCondition(List<SearchConditionItem> dto)
        {
            dto.Add(new SearchConditionItem
            {
                FieldName = "IsDelete",
                ConditionType = SearchTypeEnum.Equal,
                Value = "0",
                WhereType = SearchWhereTypeEnum.And
            });
            var data = repository.GetByCondition(dto, null, false);
            return data;
        }
    }
}
