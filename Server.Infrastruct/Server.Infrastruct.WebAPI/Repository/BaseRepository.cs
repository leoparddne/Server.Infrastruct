using Common.Toolkit.Helper;
using Server.Domain.Dto.InDto.Search;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Enums.Search;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.CollectionExtension;
using Server.Infrastruct.WebAPI.Helper;
using Server.Infrastruct.WebAPI.Model;
using Server.Infrastruct.WebAPI.Repository.Filter;
using Server.Infrastruct.WebAPI.UnitOfWork;
using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace Server.Infrastruct.WebAPI.Repository
{
    public class BaseRepository<T> : BaseRepositoryExtension, IBaseRepository<T> where T : class, new()
    {

        public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Simple Query
        /// <summary>
        /// 根据主键查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        public T QueryById(object pkValue)
        {
            return sqlSugarClient.Queryable<T>().InSingle(pkValue);
        }

        /// <summary>
        /// 根据主键值列表查询数据
        /// </summary>
        /// <param name="pkValueList">主键值列表</param>
        /// <returns></returns>
        public List<T> QueryByIdList<PkType>(List<PkType> pkValueList)
        {
            return sqlSugarClient.Queryable<T>().In(pkValueList).ToList();
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> QueryAllList()
        {
            return sqlSugarClient.Queryable<T>().ToList();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<T> QueryListByCondition(Expression<Func<T, bool>> whereCondition,
                                            Expression<Func<T, object>> orderByCondition = null,
                                            OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .ToList();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public ISugarQueryable<T> QueryableListByCondition(Expression<Func<T, bool>> whereCondition,
                                            Expression<Func<T, object>> orderByCondition = null,
                                            OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable">为null时将从上下文创建</param>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ISugarQueryable<T> QueryListByCondition(ISugarQueryable<T> queryable, string where, object parameters)
        {
            return queryable ?? sqlSugarClient.Queryable<T>()
                                    .Where(where, parameters);
        }

        public List<T> ToList(ISugarQueryable<T> queryable,
            Expression<Func<T, object>> orderByCondition = null,
                                            OrderByType orderByType = OrderByType.Asc)
        {
            if (queryable == null)
            {
                return sqlSugarClient.Queryable<T>()
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .ToList();
            }
            return queryable.OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .ToList();
        }

        public PageModel<T> ToPage<T>(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition,
                                          int pageIndex = 1,
                                          int pageSize = 20,
                                          OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<T> list;
            if (queryable == null)
            {
                queryable = sqlSugarClient.Queryable<T>();
            }

            if (orderByCondition != null)
            {
                queryable = queryable.OrderBy(orderByCondition, orderByType);
            }

            list = queryable.ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<T>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryListByCondition<TResult>(Expression<Func<T, TResult>> selectFields,
                                                           Expression<Func<T, bool>> whereCondition,
                                                           Expression<Func<T, object>> orderByCondition,
                                                           OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 根据条件查询数据(去重)
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryListByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields,
                                                                       Expression<Func<T, bool>> whereCondition,
                                                                       Expression<Func<T, object>> orderByCondition = null,
                                                                       OrderByType orderByType = OrderByType.Asc)
        {
            return QueryQueryableByConditionWithDistinct(selectFields, whereCondition, orderByCondition, orderByType)
                                   .ToList();
        }

        /// <summary>
        /// 根据条件查询数据(去重)
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns>Queryable</returns>
        public ISugarQueryable<TResult> QueryQueryableByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields,
                                                                       Expression<Func<T, bool>> whereCondition,
                                                                       Expression<Func<T, object>> orderByCondition = null,
                                                                       OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                   .WhereIF(whereCondition != null, whereCondition)
                                   .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                   .Distinct()
                                   .Select(selectFields);
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public T QuerySingleByCondition(Expression<Func<T, bool>> whereCondition,
                                        Expression<Func<T, object>> orderByCondition = null,
                                        OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .First();
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public TResult QuerySingleByCondition<TResult>(Expression<Func<T, TResult>> selectFields,
                                                       Expression<Func<T, bool>> whereCondition,
                                                       Expression<Func<T, object>> orderByCondition,
                                                       OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }
        #endregion

        #region Association Query
        /// <summary>
        /// 查询两表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                        Expression<Func<T1, T2, object[]>> joinCondition,
                                                        Expression<Func<T1, T2, bool>> whereCondition,
                                                        Expression<Func<T1, T2, object>> orderByCondition,
                                                        OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                               Expression<Func<T1, T2, object[]>> joinCondition,
                                                               Expression<Func<T1, T2, bool>> whereCondition,
                                                               Expression<Func<T1, T2, object>> orderByCondition,
                                                               OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }



        public PageModel<TResult> QueryMuchPageWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                               Expression<Func<T1, T2, object[]>> joinCondition,
                                                               Expression<Func<T1, T2, bool>> whereCondition,
                                                               Expression<Func<T1, T2, object>> orderByCondition,
                                                               OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType).Distinct()
                                    .Select(selectFields)
                                    .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                               Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                               Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                               Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                               OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }


        public PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                       Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                       Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                       Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                       OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        public PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                    Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                    Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                    Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                    OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }



        /// <summary>
        /// 查询两表关联列表数据(去重)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryMuchWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                                    Expression<Func<T1, T2, object[]>> joinCondition,
                                                                    Expression<Func<T1, T2, bool>> whereCondition,
                                                                    Expression<Func<T1, T2, object>> orderByCondition,
                                                                    OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Distinct()
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询三表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                            Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                            Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                            Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                            OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询三表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                        Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                        Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                        Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                        OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Distinct()
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询三表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="take">查询记录条数</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchWithTake<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                    Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                    Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                    Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                    int take,
                                                                    OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .Take(take)
                                    .ToList();
        }

        /// <summary>
        /// 查询四表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuch<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询四表关联列表数据(去重)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                            Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                            Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                            Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                            OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Distinct()
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询五表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuch<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                    Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                    Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                    Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                    OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();

        }

        /// <summary>
        /// 查询五表关联列表数据(去重)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                                Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                                Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                                Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                                OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Distinct()
                                    .Select(selectFields)
                                    .ToList();

        }

        /// <summary>
        /// 查询六表关联列表数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuch<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                        Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                        Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                        Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition,
                                                                        OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();

        }

        /// <summary>
        /// 查询两表关联单条数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public TResult QueryMuchSingle<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                        Expression<Func<T1, T2, object[]>> joinCondition,
                                                        Expression<Func<T1, T2, bool>> whereCondition,
                                                        Expression<Func<T1, T2, object>> orderByCondition,
                                                        OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }

        /// <summary>
        /// 查询三表关联单条数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public TResult QueryMuchSingle<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                            Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                            Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                            Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                            OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }

        /// <summary>
        /// 查询四表关联单条数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public TResult QueryMuchSingle<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }

        /// <summary>
        /// 查询五表关联单条数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public TResult QueryMuchSingle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                    Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                    Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                    Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                    OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }
        #endregion

        #region Aggregate Query
        /// <summary>
        /// 单表分组查询
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryGroupBy<TResult>(Expression<Func<T, TResult>> selectFields,
                                                   Expression<Func<T, bool>> whereCondition,
                                                   Expression<Func<T, object>> groupCondition,
                                                   Expression<Func<T, object>> orderByCondition,
                                                   OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .GroupBy(groupCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询二表关联列表数据(GroupBy)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchGroupBy<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                               Expression<Func<T1, T2, object[]>> joinCondition,
                                                               Expression<Func<T1, T2, bool>> whereCondition,
                                                               Expression<Func<T1, T2, object>> groupCondition,
                                                               Expression<Func<T1, T2, object>> orderByCondition,
                                                               OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .GroupBy(groupCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询三表关联列表数据(GroupBy)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public List<TResult> QueryMuchGroupBy<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                   Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                   Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                   Expression<Func<T1, T2, T3, object>> groupCondition,
                                                                   Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                   OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .GroupBy(groupCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }

        /// <summary>
        /// 查询三表关联列表数据(GroupBy)
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param> 
        /// <returns></returns>
        public ISugarQueryable<TResult> QueryMuchGroupByToQueryable<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                   Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                   Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                   Expression<Func<T1, T2, T3, object>> groupCondition,
                                                                   Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                   OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable(joinCondition)
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .GroupBy(groupCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields);
        }

        public PageModel<TResult> ToPage<T1, T2, T3, TResult>(ISugarQueryable<TResult> sugarQueryable, int pageIndex = 1,
                                          int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sugarQueryable.ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 单表分组查询
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public TResult QuerySingleGroupBy<TResult>(Expression<Func<T, TResult>> selectFields,
                                                   Expression<Func<T, bool>> whereCondition,
                                                   Expression<Func<T, object>> groupCondition,
                                                   Expression<Func<T, object>> orderByCondition,
                                                   OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .WhereIF(whereCondition != null, whereCondition)
                                    .GroupBy(groupCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .First();
        }

        /// <summary>
        /// 单表分组查询
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="groupCondition">分组表达式</param>
        /// <param name="havingCondition">聚合条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public List<TResult> QueryGroupByHaving<TResult>(Expression<Func<T, TResult>> selectFields,
                                                         Expression<Func<T, object>> groupCondition,
                                                         Expression<Func<T, bool>> havingCondition,
                                                         Expression<Func<T, object>> orderByCondition,
                                                         OrderByType orderByType = OrderByType.Asc)
        {
            return sqlSugarClient.Queryable<T>()
                                    .GroupBy(groupCondition)
                                    .Having(havingCondition)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .Select(selectFields)
                                    .ToList();
        }
        #endregion

        #region Insert
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            return sqlSugarClient.Insertable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 批量写入实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        public int InsertRange(List<T> entityList)
        {
            return sqlSugarClient.Insertable(entityList).ExecuteCommand();
        }
        #endregion

        /// <summary>
        /// 根据条件新增或更新数据
        /// 根据条件查询数据,如果存在此数据则执行更新逻辑,否则执行新增
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereCondition"></param>
        public void CreateOrUpdate(T entity, Expression<Func<T, bool>> whereCondition)
        {
            if (Exists(whereCondition))
            {
                Update(entity, whereCondition);
            }
            else
            {
                Insert(entity);
            }
        }

        #region Update
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return sqlSugarClient.Updateable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 根据条件更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        public bool Update(T entity, Expression<Func<T, bool>> whereCondition)
        {
            return sqlSugarClient.Updateable(entity).Where(whereCondition).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 更新某几个字段
        /// </summary>
        /// <param name="columns">字段表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereCondition)
        {
            return sqlSugarClient.Updateable<T>().SetColumns(columns).Where(whereCondition).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        public bool UpdateRange(List<T> entityList)
        {
            return sqlSugarClient.Updateable(entityList).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 批量更新指定字段实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        ///  <param name="columns">不更新字段表达式</param>
        /// <returns></returns>
        public bool UpdateRange(List<T> entityList, Expression<Func<T, object>> columns)
        {
            return sqlSugarClient.Updateable(entityList).IgnoreColumns(columns).ExecuteCommandHasChange();
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            return sqlSugarClient.Deleteable(entity).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> whereCondition)
        {
            return sqlSugarClient.Deleteable(whereCondition).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        public bool DeleteById(object pkValue)
        {
            return sqlSugarClient.Deleteable<T>(pkValue).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 批量删除指定主键数据
        /// </summary>
        /// <param name="pdValueList">主键值列表</param>
        /// <returns></returns>
        public bool DeleteByIdList<PkType>(List<PkType> pdValueList)
        {
            return sqlSugarClient.Deleteable<T>().In(pdValueList).ExecuteCommandHasChange();
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        public bool DeleteRange(List<T> entityList)
        {
            return sqlSugarClient.Deleteable(entityList).ExecuteCommandHasChange();
        }
        #endregion

        #region Exists
        /// <summary>
        /// 判断数据是否存在
        /// True-存在
        /// False-不存在
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return sqlSugarClient.Queryable<T>().Where(whereCondition).Any();
        }
        #endregion

        #region Calculate
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Queryable<T>().Count(condition);
        }

        /// <summary>
        /// 获取数据某个字段的合计
        /// </summary>
        /// <typeparam name="TResult">字段类型</typeparam>
        /// <param name="field">字段</param>
        /// <param name="condition">条件表达式</param>
        /// <returns></returns>
        public TResult GetSum<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Sum(field);
        }

        /// <summary>
        /// 获取某个字段最大值
        /// </summary>
        /// <typeparam name="TResult">字段类型</typeparam>
        /// <param name="field">字段</param>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public TResult GetMax<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Max(field);
        }



        /// <summary>
        /// 获取某个字段最小值
        /// </summary>
        /// <typeparam name="TResult">字段类型</typeparam>
        /// <param name="field">字段</param>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        public TResult GetMin<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition)
        {
            return sqlSugarClient.Queryable<T>().WhereIF(condition != null, condition).Min(field);
        }
        #endregion

        #region Page
        /// <summary>
        /// 单表查询分页数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<T> QueryPageList(Expression<Func<T, bool>> whereCondition,
                                          Expression<Func<T, object>> orderByCondition,
                                          int pageIndex = 1,
                                          int pageSize = 20,
                                          OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<T> list = sqlSugarClient.Queryable<T>()
                                            .WhereIF(whereCondition != null, whereCondition)
                                            .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                            .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<T>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 单表查询分页数据
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<TResult>(Expression<Func<T, TResult>> selectFields,
                                                         Expression<Func<T, bool>> whereCondition,
                                                         Expression<Func<T, object>> orderByCondition,
                                                         int pageIndex = 1,
                                                         int pageSize = 20,
                                                         OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;
            List<TResult> list = sqlSugarClient.Queryable<T>()
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);

            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }


        /// <summary>
        /// 两表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                                 Expression<Func<T1, T2, object[]>> joinCondition,
                                                                 Expression<Func<T1, T2, bool>> whereCondition,
                                                                 Expression<Func<T1, T2, object>> orderByCondition,
                                                                 int pageIndex = 1,
                                                                 int pageSize = 20,
                                                                 OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 两表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                                 Expression<Func<T1, T2, object[]>> joinCondition,
                                                                 Expression<Func<T1, T2, bool>> whereCondition,
                                                                 string orderFileds,
                                                                 int pageIndex = 1,
                                                                 int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 两表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                     Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                     Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                     Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                     int pageIndex = 1,
                                                                     int pageSize = 20,
                                                                     OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }


        /// <summary>
        /// 三表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                     Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                     Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                     string orderFileds,
                                                                     int pageIndex = 1,
                                                                     int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 四表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                         Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                         Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                         Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                         int pageIndex = 1,
                                                                         int pageSize = 20,
                                                                         OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 四表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                         Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                         Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                         string orderFileds,
                                                                         int pageIndex = 1,
                                                                         int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 五表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                             Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                             Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                             Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                             int pageIndex = 1,
                                                                             int pageSize = 20,
                                                                             OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 五表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                             Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                             Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                             string orderFileds,
                                                                             int pageIndex = 1,
                                                                             int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 六表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition,
                                                                                 int pageIndex = 1,
                                                                                 int pageSize = 20,
                                                                                 OrderByType orderByType = OrderByType.Asc)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }

        /// <summary>
        /// 六表关联查询分页数据
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="joinCondition">关联表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderFileds">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                                 string orderFileds,
                                                                                 int pageIndex = 1,
                                                                                 int pageSize = 20)
        {
            int totalCount = 0;
            int totalPages = 0;

            List<TResult> list = sqlSugarClient.Queryable(joinCondition)
                                                .WhereIF(whereCondition != null, whereCondition)
                                                .OrderByIF(!string.IsNullOrWhiteSpace(orderFileds), orderFileds)
                                                .Select(selectFields)
                                                .ToPageList(pageIndex, pageSize, ref totalCount);


            if (pageSize > 0 && totalCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            }
            return new PageModel<TResult>() { TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, DataList = list };
        }
        #endregion

        /// <summary>
        /// 动态参数查询
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="orderByCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldNameISDBName"></param>
        /// <param name="orderByType"></param>
        /// <param name="expEx"></param>
        /// <returns></returns>
        public PageModel<T> GetPageModelByCondition(List<SearchConditionItem> conditions, Expression<Func<T, object>> orderByCondition, int pageIndex, int pageSize, bool fieldNameISDBName = true, OrderByType orderByType = OrderByType.Asc, Expression<Func<T, bool>> expEx = null)
        {
            ISugarQueryable<T> sugarQueryable = GetQueryableByCondition(conditions, fieldNameISDBName, expEx);

            //return partRepository.QueryListByCondition(f => f.PartId == "53EAF25A88CE46F7B63E0C9C239F6A9D");
            return ToPage(sugarQueryable, orderByCondition, pageIndex, pageSize, orderByType);
            //return partRepository.ToList(sugarQueryable);
        }

        public List<T> GetByCondition(List<SearchConditionItem> conditions, Expression<Func<T, object>> orderByCondition, bool fieldNameISDBName = true, OrderByType orderByType = OrderByType.Asc, Expression<Func<T, bool>> expEx = null)
        {
            ISugarQueryable<T> sugarQueryable = GetQueryableByCondition(conditions, fieldNameISDBName, expEx);

            return sqlSugarClient.Queryable(sugarQueryable)
                                    .OrderByIF(orderByCondition != null, orderByCondition, orderByType)
                                    .ToList();
        }

        private ISugarQueryable<T> GetQueryableByCondition(List<SearchConditionItem> conditions, bool fieldNameISDBName, Expression<Func<T, bool>> expEx)
        {
            //SqlFunc
            ISugarQueryable<T> sugarQueryable = null;
            var exp = Expressionable.Create<T>();
            if (expEx != null)
            {
                //TODO 后续扩展or的情况
                exp = exp.And(expEx);
            }

            //var exp = ExpressionHelper.True<PartEntity>();
            if (conditions != null)
            {

                foreach (SearchConditionItem condition in conditions)
                {
                    if (string.IsNullOrEmpty(condition.FieldName)) continue;
                    //TODO 扩展根据不同类型执行不同的拼接数据格式

                    //字段名称是否对应数据库字段
                    string sqlFieldName = fieldNameISDBName ? condition.FieldName//.ToUpper()
                                        : SugarEntityHelper.GetSugarNameByEntityName<T>(condition.FieldName);

                    ExceptionHelper.CheckException(sqlFieldName == null, MessageEnum.FieldNotFound);

                    Type fieldType = SugarEntityHelper.GetSugarFieldTypeByName<T>(sqlFieldName);
                    if (fieldType == null)
                    {
                        throw new Exception($"Error type {condition.FieldName}");
                    }

                    var dbType = SqlSugarCollectionExtension.GetDBType();
                    string dbFieldName = dbType == DbType.Oracle ? sqlFieldName.ToUpper() : sqlFieldName;
                    string calcValue = condition.Value;

                    //处理like逻辑
                    calcValue = ContainerProcess(condition, calcValue);
                    string valueData = GetValueWithNullable(fieldType, calcValue);
                    //sugarQueryable = AddWhere(sugarQueryable, condition, dbFieldName, valueData);


                    var whereStr = GenerageWhere(condition, dbFieldName, valueData);
                    switch (condition.WhereType)
                    {
                        case SearchWhereTypeEnum.And:
                            exp = exp.And(f => SqlFunc.MappingColumn(default(bool), whereStr));
                            break;
                        case SearchWhereTypeEnum.Or:
                            exp = exp.Or(f => SqlFunc.MappingColumn(default(bool), whereStr));
                            break;
                        default:
                            break;
                    }
                }
            }
            if (sugarQueryable == null)
            {
                sugarQueryable = sqlSugarClient.Queryable<T>();
            }

            sugarQueryable.Where(exp.ToExpression());
            return sugarQueryable;
        }

        private static string GetValueWithNullable(Type fieldType, string calcValue)
        {
            if (fieldType.IsGenericType)
            {
                var genericArgc = fieldType.GetGenericArguments();
                if (genericArgc.Length > 0)
                {
                    return GetValue(genericArgc.First(), calcValue);
                }
                throw new Exception($"ErrorNullableArgc");
            }
            else
            {
                return GetValue(fieldType, calcValue);
            }
        }

        public static string GetValue(Type fieldType, string calcValue)
        {
            switch (fieldType.Name)
            {
                case nameof(String):
                    return "'" + calcValue + "'";
                    break;
                case nameof(DateTime):
                    DbType dbType;
                    if (Enum.TryParse<DbType>(DBConfigSingleton.GetConfig().ConnectionType, out dbType))
                    {
                        switch (dbType)
                        {
                            case DbType.MySql:
                                break;
                            case DbType.SqlServer:
                                break;
                            case DbType.Sqlite:
                                break;
                            case DbType.Oracle:
                                return $"TO_DATE('{calcValue}','yyyy-MM-dd HH24:MI:SS')";
                                break;
                            case DbType.PostgreSQL:
                                break;
                            case DbType.Dm:
                                break;
                            case DbType.Kdbndp:
                                break;
                            case DbType.Oscar:
                                break;
                            case DbType.MySqlConnector:
                                break;
                            case DbType.Access:
                                break;
                            case DbType.OpenGauss:
                                break;
                            case DbType.QuestDB:
                                break;
                            case DbType.HG:
                                break;
                            case DbType.ClickHouse:
                                break;
                            case DbType.GBase:
                                break;
                            case DbType.Odbc:
                                break;
                            case DbType.Custom:
                                break;
                            default:
                                break;
                        }

                        return $"'{calcValue}'";
                    }

                    throw new Exception("ErrorDBType");
                    break;

                default:
                    break;
            }

            return calcValue;


            ////拼接字符串外侧引号
            //return fieldType == typeof(string) ? ("'" + calcValue + "'") : calcValue;
        }

        private string GenerageWhere(SearchConditionItem condition, string dbFieldName, string valueData)
        {
            switch (condition.ConditionType)
            {
                case SearchTypeEnum.Equal:
                    return @$" ""{dbFieldName}""={valueData} ";
                case SearchTypeEnum.NotEqual:
                    return @$" ""{dbFieldName}""!={valueData} ";
                case SearchTypeEnum.GreaterThan:
                    return @$" ""{dbFieldName}"">{valueData} ";
                case SearchTypeEnum.GreaterThanOrEqual:
                    return @$" ""{dbFieldName}"">={valueData} ";
                case SearchTypeEnum.LessThan:
                    return @$" ""{dbFieldName}""<{valueData} ";
                case SearchTypeEnum.LessThanOrEqual:
                    return @$" ""{dbFieldName}""<={valueData} ";
                case SearchTypeEnum.Container:
                    return @$" ""{dbFieldName}"" like {valueData.Replace("_", "\\_")} escape '\'";
                case SearchTypeEnum.ContainerLeft:
                    return @$" ""{dbFieldName}"" like {valueData.Replace("_", "\\_")} escape '\'";
                case SearchTypeEnum.ContainerRight:
                    return @$" ""{dbFieldName}"" like {valueData.Replace("_", "\\_")} escape '\'";
                default:
                    break;
            }
            return "";
        }

        /// <summary>
        /// 动态查询单个条件
        /// </summary>
        /// <param name="sugarQueryable"></param>
        /// <param name="condition"></param>
        /// <param name="dbFieldName"></param>
        /// <param name="valueData"></param>
        /// <returns></returns>
        private ISugarQueryable<T> AddWhere(ISugarQueryable<T> sugarQueryable, SearchConditionItem condition, string dbFieldName, string valueData)
        {
            //ConditionalModel conditionalModel = new ConditionalModel();
            //conditionalModel.ConditionalType= ConditionalType.
            //ConditionalType
            switch (condition.ConditionType)
            {
                case SearchTypeEnum.Equal:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}""={valueData}", null);
                    break;
                case SearchTypeEnum.NotEqual:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}""!={valueData}", null);
                    break;
                case SearchTypeEnum.GreaterThan:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}"">{valueData}", null);
                    break;
                case SearchTypeEnum.GreaterThanOrEqual:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}"">={valueData}", null);
                    break;
                case SearchTypeEnum.LessThan:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}""<{valueData}", null);
                    break;
                case SearchTypeEnum.LessThanOrEqual:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}""<={valueData}", null);
                    break;
                case SearchTypeEnum.Container:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}"" like {valueData}", null);
                    break;
                case SearchTypeEnum.ContainerLeft:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}"" like {valueData}", null);
                    break;
                case SearchTypeEnum.ContainerRight:
                    sugarQueryable = QueryListByCondition(sugarQueryable, @$"""{dbFieldName}"" like {valueData}", null);
                    break;
                default:
                    break;
            }

            return sugarQueryable;
        }

        /// <summary>
        /// 处理like逻辑
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="calcValue"></param>
        /// <returns></returns>
        private static string ContainerProcess(SearchConditionItem condition, string calcValue)
        {
            switch (condition.ConditionType)
            {
                case SearchTypeEnum.Container:
                    calcValue = $"%{calcValue}%";
                    break;
                case SearchTypeEnum.ContainerLeft:
                    calcValue = $"{calcValue}%";
                    break;
                case SearchTypeEnum.ContainerRight:
                    calcValue = $"%{calcValue}";
                    break;
                default:
                    break;
            }

            return calcValue;
        }

        /// <summary>
        /// 保存历史表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreDesc">是否需要使用automap映射数据到非同名字段</param>
        public void SaveHistory(T obj, bool ignoreDesc = false)
        {
            //获取绑定的特性
            object[] attrs = typeof(T).GetCustomAttributes(false);
            foreach (object attr in attrs)
            {
                if (attr is HistoryBindAttribute bindAttribute)
                {
                    object newEntity = MapperHelper.AutoMapByType(obj, bindAttribute.DBEntityType, false);

                    Dictionary<string, object> map = new Dictionary<string, object>();

                    // 获取对象对应的类， 对应的类型
                    PropertyInfo[] pi = newEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

                    foreach (PropertyInfo p in pi)
                    {
                        MethodInfo m = p.GetGetMethod();
                        string sugarFieldName = SugarEntityHelper.GetSugarNameByEntityNamByObj(newEntity, p.Name);
                        map.Add(sugarFieldName.ToUpper(), p.GetValue(newEntity));
                    }

                    string tableName = SugarEntityHelper.GetTableName(bindAttribute.DBEntityType);
                    sqlSugarClient.Insertable(map).AS(tableName).ExecuteCommand();
                }
            }
        }

        /// <summary>
        /// 获取仓储对应表名称
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            return SugarEntityHelper.GetTableName(typeof(T));
        }
    }
}
