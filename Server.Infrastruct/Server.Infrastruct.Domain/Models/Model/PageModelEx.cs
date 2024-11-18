using Common.Toolkit.Extention;

namespace Server.Infrastruct.Model.Models.Model
{
    public static class PageModelEx
    {
        public static bool IsNullOrEmpty<T>(this PageModel<T> pageModel)
        {
            return pageModel == null || pageModel.DataList.IsNullOrEmpty();
        }
    }
}
