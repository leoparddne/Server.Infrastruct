using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.Repository.Common.Ex
{
    public static class SqlsugarAbleEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateable"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IUpdateable<T> Update(this IUpdateable<T> updateable, Expression<Func<T, object>> columns)
        {
            return updateable.UpdateColumns(columns);
        }

    }
}
