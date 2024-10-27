using SqlSugar;
using System.Linq.Expressions;

namespace Server.Infrastruct.Extension.SqlSugar
{
    public static class SqlsugarAbleEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateable"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IUpdateable<T> Update<T>(this IUpdateable<T> updateable, Expression<Func<T, object>> columns) where T : class, new()
        {
            return updateable.UpdateColumns(columns);
        }

    }
}
