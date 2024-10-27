using Common.Toolkit.Helper;
using Server.Infrastruct.Model.Models.Model;

namespace Server.Infrastruct.Model.Helper
{
    public static class PageModelHelper
    {
        public static PageModel<Result> Map<T, Result>(this PageModel<T> origin) where Result : new()
        {
            if (origin == null)
            {
                return null;
            }

            PageModel<Result> result = new();

            result.PageIndex = origin.PageIndex;
            result.PageSize = origin.PageSize;
            result.TotalCount = origin.TotalCount;
            result.TotalPages = origin.TotalPages;

            result.DataList = origin.DataList.AutoMap<T, Result>()?.ToList();

            return result;
        }
    }
}
