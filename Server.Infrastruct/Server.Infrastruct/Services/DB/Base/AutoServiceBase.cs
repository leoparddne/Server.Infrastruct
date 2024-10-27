using Server.Infrastruct.Repository;

namespace Server.Infrastruct.Services.DB.Base
{
    /// <summary>
    /// 提供对于实体的泛型仓储 - 不需要手动建立仓储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoServiceBase<T> : IAutoServiceBase<T> where T : class
    {
        public IBaseRepository<T> Repository { get; set; }

        public AutoServiceBase(IBaseRepository<T> repository)
        {
            Repository = repository;
        }

    }
}
