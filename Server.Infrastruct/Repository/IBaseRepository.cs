using Server.Infrastruct.Model.Dto.InDto.Search;
using Server.Infrastruct.Model.Models.Model;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.Repository
{
    public interface IBaseRepository<T> : IBaseRepositoryExtension where T : class
    {
        #region Simple Query
        /// <summary>
        /// 根据主键查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        T QueryById(object pkValue);

        /// <summary>
        /// 根据主键值列表查询数据
        /// </summary>
        /// <param name="pkValueList">主键值列表</param>
        /// <returns></returns>
        List<T> QueryByIdList<PkType>(List<PkType> pkValueList);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        List<T> QueryAllList();

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        List<T> QueryListByCondition(Expression<Func<T, bool>> whereCondition,
                                     Expression<Func<T, object>>? orderByCondition = null,
                                     OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        List<TResult> QueryListByCondition<TResult>(Expression<Func<T, TResult>> selectFields,
                                                    Expression<Func<T, bool>> whereCondition,
                                                    Expression<Func<T, object>> orderByCondition,
                                                    OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据(去重)
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        List<TResult> QueryListByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields,
                                                                Expression<Func<T, bool>> whereCondition,
                                                                Expression<Func<T, object>> orderByCondition,
                                                                OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据(去重)
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns>Queryable</returns>
        ISugarQueryable<TResult> QueryQueryableByConditionWithDistinct<TResult>(Expression<Func<T, TResult>> selectFields,
                                                                Expression<Func<T, bool>> whereCondition,
                                                                Expression<Func<T, object>> orderByCondition,
                                                                OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        T QuerySingleByCondition(Expression<Func<T, bool>> whereCondition,
                                 Expression<Func<T, object>> orderByCondition = null,
                                 OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="selectFields">返回字段</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        TResult QuerySingleByCondition<TResult>(Expression<Func<T, TResult>> selectFields,
                                                Expression<Func<T, bool>> whereCondition,
                                                Expression<Func<T, object>> orderByCondition,
                                                OrderByType orderByType = OrderByType.Asc);
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
        List<TResult> QueryMuch<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                 Expression<Func<T1, T2, object[]>> joinCondition,
                                                 Expression<Func<T1, T2, bool>> whereCondition,
                                                 Expression<Func<T1, T2, object>> orderByCondition,
                                                 OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                             Expression<Func<T1, T2, object[]>> joinCondition,
                                                             Expression<Func<T1, T2, bool>> whereCondition,
                                                             Expression<Func<T1, T2, object>> orderByCondition,
                                                             OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                     Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                     Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                     Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                     OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                                 Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                                 Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                                 Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                                 OrderByType orderByType = OrderByType.Asc);


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
        List<TResult> QueryMuchWithTake<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                             Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                             Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                             Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                             int take,
                                                             OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                         Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                         Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                         Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                         OrderByType orderByType = OrderByType.Asc);
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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                     Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                     Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                     Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                     OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                             Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                             Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                             Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                             OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchWithDistinct<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                         Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                         Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                         Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                         OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuch<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                 Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition,
                                                                 OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                 Expression<Func<T1, T2, object[]>> joinCondition,
                                                 Expression<Func<T1, T2, bool>> whereCondition,
                                                 Expression<Func<T1, T2, object>> orderByCondition,
                                                 OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                     Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                     Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                     Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                     OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                         Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                         Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                         Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                         OrderByType orderByType = OrderByType.Asc);

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
        TResult QueryMuchSingle<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                             Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                             Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                             Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                             OrderByType orderByType = OrderByType.Asc);
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
        List<TResult> QueryGroupBy<TResult>(Expression<Func<T, TResult>> selectFields,
                                            Expression<Func<T, bool>> whereCondition,
                                            Expression<Func<T, object>> groupCondition,
                                            Expression<Func<T, object>> orderByCondition,
                                            OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchGroupBy<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                        Expression<Func<T1, T2, object[]>> joinCondition,
                                                        Expression<Func<T1, T2, bool>> whereCondition,
                                                        Expression<Func<T1, T2, object>> groupCondition,
                                                        Expression<Func<T1, T2, object>> orderByCondition,
                                                        OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryMuchGroupBy<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                            Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                            Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                            Expression<Func<T1, T2, T3, object>> groupCondition,
                                                            Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                            OrderByType orderByType = OrderByType.Asc);

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
        TResult QuerySingleGroupBy<TResult>(Expression<Func<T, TResult>> selectFields,
                                            Expression<Func<T, bool>> whereCondition,
                                            Expression<Func<T, object>> groupCondition,
                                            Expression<Func<T, object>> orderByCondition,
                                            OrderByType orderByType = OrderByType.Asc);

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
        List<TResult> QueryGroupByHaving<TResult>(Expression<Func<T, TResult>> selectFields,
                                                  Expression<Func<T, object>> groupCondition,
                                                  Expression<Func<T, bool>> havingCondition,
                                                  Expression<Func<T, object>> orderByCondition,
                                                  OrderByType orderByType = OrderByType.Asc);

        #endregion

        #region Insert
        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        /// 批量写入实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        int InsertRange(List<T> entityList);
        #endregion

        public void CreateOrUpdate(T entity, Expression<Func<T, bool>> whereCondition);


        #region Update
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 根据条件更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        bool Update(T entity, Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// 更新某几个字段
        /// </summary>
        /// <param name="columns">字段表达式</param>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        bool UpdateRange(List<T> entityList);

        /// <summary>
        /// 批量更新实体数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <param name="columns">不更新字段</param>
        /// <returns></returns>
        bool UpdateRange(List<T> entityList, Expression<Func<T, object>> columns);
        #endregion

        #region Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// 删除指定主键数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns></returns>
        bool DeleteById(object pkValue);

        /// <summary>
        /// 批量删除指定主键数据
        /// </summary>
        /// <param name="pdValueList">主键值列表</param>
        /// <returns></returns>
        bool DeleteByIdList<PkType>(List<PkType> pdValueList);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entityList">实体类列表</param>
        /// <returns></returns>
        bool DeleteRange(List<T> entityList);
        #endregion

        #region Exists
        /// <summary>
        /// 判断数据是否存在
        /// True-存在
        /// False-不存在
        /// </summary>
        /// <param name="whereCondition">条件表达式</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> whereCondition);
        #endregion

        #region Calculate
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="condition">条件表达式</param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> condition);

        /// <summary>
        /// 获取数据某个字段的合计
        /// </summary>
        /// <typeparam name="TResult">字段类型</typeparam>
        /// <param name="field">字段</param>
        /// <param name="condition">条件表达式</param>
        /// <returns></returns>
        TResult GetSum<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition);

        /// <summary>
        /// 获取某个字段最大值
        /// </summary>
        /// <typeparam name="TResult">字段类型</typeparam>
        /// <param name="field">字段</param>
        /// <param name="condition">表达式</param>
        /// <returns></returns>
        TResult GetMax<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition);
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
        PageModel<T> QueryPageList(Expression<Func<T, bool>> whereCondition,
                                   Expression<Func<T, object>> orderByCondition,
                                   int pageIndex = 1,
                                   int pageSize = 20,
                                   OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<TResult>(Expression<Func<T, TResult>> selectFields,
                                                  Expression<Func<T, bool>> whereCondition,
                                                  Expression<Func<T, object>> orderByCondition,
                                                  int pageIndex = 1,
                                                  int pageSize = 20,
                                                  OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                          Expression<Func<T1, T2, object[]>> joinCondition,
                                                          Expression<Func<T1, T2, bool>> whereCondition,
                                                          Expression<Func<T1, T2, object>> orderByCondition,
                                                          int pageIndex = 1,
                                                          int pageSize = 20,
                                                          OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                          Expression<Func<T1, T2, object[]>> joinCondition,
                                                          Expression<Func<T1, T2, bool>> whereCondition,
                                                          string orderFileds,
                                                          int pageIndex = 1,
                                                          int pageSize = 20);

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
        /// <param name="orderByCondition">排序表达式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderByType">排序顺序(asc/desc)</param>
        /// <returns></returns>
        PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                              Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                              Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                              Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                              int pageIndex = 1,
                                                              int pageSize = 20,
                                                              OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                              Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                              Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                              string orderFileds,
                                                              int pageIndex = 1,
                                                              int pageSize = 20);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                  Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                  Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                  Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                                  int pageIndex = 1,
                                                                  int pageSize = 20,
                                                                  OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                                  Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                                  Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                                  string orderFileds,
                                                                  int pageIndex = 1,
                                                                  int pageSize = 20);


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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                      Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                      Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                      Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                                      int pageIndex = 1,
                                                                      int pageSize = 20,
                                                                      OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                                      Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                                      Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                                      string orderFileds,
                                                                      int pageIndex = 1,
                                                                      int pageSize = 20);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                          Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                          Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                          Expression<Func<T1, T2, T3, T4, T5, T6, object>> orderByCondition,
                                                                          int pageIndex = 1,
                                                                          int pageSize = 20,
                                                                          OrderByType orderByType = OrderByType.Asc);

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
        PageModel<TResult> QueryPageList<T1, T2, T3, T4, T5, T6, TResult>(Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> selectFields,
                                                                          Expression<Func<T1, T2, T3, T4, T5, T6, object[]>> joinCondition,
                                                                          Expression<Func<T1, T2, T3, T4, T5, T6, bool>> whereCondition,
                                                                          string orderFileds,
                                                                          int pageIndex = 1,
                                                                          int pageSize = 20);
        List<T> ToList(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);
        ISugarQueryable<T> QueryListByCondition(ISugarQueryable<T> queryable, string where, object parameters);
        PageModel<T> ToPage<T>(ISugarQueryable<T> queryable, Expression<Func<T, object>> orderByCondition, int pageIndex = 1, int pageSize = 20, OrderByType orderByType = OrderByType.Asc);

        /// <summary>
        /// 动态查询
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="orderByCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldNameISDBName">查询字段名称是否为数据库名称</param>
        /// <param name="orderByType"></param>
        /// <param name="expEx"></param>
        /// <returns></returns>
        PageModel<T> GetPageModelByCondition(List<SearchConditionItem> conditions, Expression<Func<T, object>> orderByCondition, int pageIndex, int pageSize, bool fieldNameISDBName = true, OrderByType orderByType = OrderByType.Asc, Expression<Func<T, bool>> expEx = null);
        PageModel<TResult> QueryMuchPage<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields, Expression<Func<T1, T2, object[]>> joinCondition, Expression<Func<T1, T2, bool>> whereCondition, Expression<Func<T1, T2, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20);

        PageModel<TResult> QueryMuchPageWithDistinct<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selectFields,
                                                              Expression<Func<T1, T2, object[]>> joinCondition,
                                                              Expression<Func<T1, T2, bool>> whereCondition,
                                                              Expression<Func<T1, T2, object>> orderByCondition,
                                                              OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20);

        PageModel<TResult> QueryMuchPage<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields,
                                                               Expression<Func<T1, T2, T3, object[]>> joinCondition,
                                                               Expression<Func<T1, T2, T3, bool>> whereCondition,
                                                               Expression<Func<T1, T2, T3, object>> orderByCondition,
                                                               OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20);

        PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> selectFields,
                                                       Expression<Func<T1, T2, T3, T4, object[]>> joinCondition,
                                                       Expression<Func<T1, T2, T3, T4, bool>> whereCondition,
                                                       Expression<Func<T1, T2, T3, T4, object>> orderByCondition,
                                                       OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20);

        PageModel<TResult> QueryMuchPage<T1, T2, T3, T4, T5, TResult>(Expression<Func<T1, T2, T3, T4, T5, TResult>> selectFields,
                                                   Expression<Func<T1, T2, T3, T4, T5, object[]>> joinCondition,
                                                   Expression<Func<T1, T2, T3, T4, T5, bool>> whereCondition,
                                                   Expression<Func<T1, T2, T3, T4, T5, object>> orderByCondition,
                                                   OrderByType orderByType = OrderByType.Asc, int pageIndex = 1, int pageSize = 20);

        /// <summary>
        /// 保存历史表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreDesc">是否需要使用automap映射数据到非同名字段</param>
        void SaveHistory(T obj, bool ignoreDesc = false);
        ISugarQueryable<TResult> QueryMuchGroupByToQueryable<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selectFields, Expression<Func<T1, T2, T3, object[]>> joinCondition, Expression<Func<T1, T2, T3, bool>> whereCondition, Expression<Func<T1, T2, T3, object>> groupCondition, Expression<Func<T1, T2, T3, object>> orderByCondition, OrderByType orderByType = OrderByType.Asc);
        PageModel<TResult> ToPage<T1, T2, T3, TResult>(ISugarQueryable<TResult> sugarQueryable, int pageIndex = 1, int pageSize = 20);
        TResult GetMin<TResult>(Expression<Func<T, TResult>> field, Expression<Func<T, bool>> condition);
        ISugarQueryable<T> QueryableListByCondition(Expression<Func<T, bool>> whereCondition, Expression<Func<T, object>> orderByCondition = null, OrderByType orderByType = OrderByType.Asc);
        List<T> GetByCondition(List<SearchConditionItem> conditions, Expression<Func<T, object>> orderByCondition, bool fieldNameISDBName = true, OrderByType orderByType = OrderByType.Asc, Expression<Func<T, bool>> expEx = null);
        string GetTableName();
        #endregion

    }
}
