using Common.Toolkit.Helper;
using Server.Infrastruct.Model.Dto.InDto;
using Server.Infrastruct.Model.Dto.InDto.Search;
using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Enums;
using Server.Infrastruct.Model.Models.Enums.Search;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.DB.Base;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.Services.DB
{
    /// <summary>
    /// 基础service，根据实体提供基础增删改查
    /// 不处理用户相关的逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoService<T> : AutoServiceBase<T>, IAutoService<T> where T : CommonEntity, new()
    {
        //public IBaseRepository<T> Repository { get; set; }

        public AutoService(IBaseRepository<T> repository) : base(repository)
        {
            //this.Repository = repository;
        }

        public void Create(T entity)
        {
            if (string.IsNullOrWhiteSpace(entity.ID))
            {
                entity.ID = GUIDHelper.NewGuid;
            }

            Repository.Insert(entity);
        }
        
        public void Create(List<T> entity)
        {
            foreach (var item in entity)
            {
                if (string.IsNullOrWhiteSpace(item.ID))
                {
                    item.ID = GUIDHelper.NewGuid;
                }
            }

            Repository.InsertRange(entity);
        }


        public void Delete(string id)
        {
            Repository.DeleteById(id);
        }

        public void Delete(List<string> ids)
        {
            Repository.DeleteByIdList(ids);
        }

        
        public void Delete(Expression<Func<T, bool>> whereCondition)
        {
            Repository.Delete(whereCondition);
        }

        public void Update(List<T> entity)
        {
            Repository.UpdateRange(entity);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
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
        /// 检查数据是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(string id)
        {
            return Repository.Exists(l => l.ID == id);
        }
        public bool Exist(Expression<Func<T, bool>> whereCondition)
        {
            return Repository.Exists(whereCondition);
        }

        public T Single(Expression<Func<T, bool>> whereCondition,
                                 Expression<Func<T, object>>? orderByCondition = null,
                                 OrderByType orderByType = OrderByType.Asc)
        {
            return Repository.QuerySingleByCondition(whereCondition, orderByCondition, orderByType);
        }

        public List<T> Get(Expression<Func<T, bool>> whereCondition,
                                  Expression<Func<T, object>>? orderByCondition = null,
                                  OrderByType orderByType = OrderByType.Asc)
        {
            return Repository.QueryListByCondition(whereCondition, orderByCondition, orderByType);
        }


        public PageModel<T> Get(Expression<Func<T, bool>> whereCondition,
                                   Expression<Func<T, object>> orderByCondition,
                                   int pageIndex = 1,
                                   int pageSize = 20,
                                   OrderByType orderByType = OrderByType.Asc)
        {
            return Repository.QueryPageList(whereCondition, orderByCondition, pageIndex, pageSize, orderByType);
        }



        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PageModel<T> GetConditionPage(SearchConditionPageInDto dto)
        {
            PageModel<T> data = Repository.GetPageModelByCondition(dto.Conditions, null, dto.PageIndex, dto.PageSize, false);
            return data;
        }


        /// <summary>
        /// 高级搜索
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<T> GetCondition(List<SearchConditionItem> dto)
        {
            var data = Repository.GetByCondition(dto, null, false);
            return data;
        }
    }
}
