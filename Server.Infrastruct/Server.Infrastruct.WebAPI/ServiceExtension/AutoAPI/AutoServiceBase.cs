using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Function;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public class AutoServiceBase<T> : IAutoCondition, IAutoCreate, IAutoDelete, IAutoSelect, IAutoUpdate, IAutoServiceBase<T> where T : class
    {
        public IBaseRepository<T> Repository { get; set; }

        public AutoServiceBase(IBaseRepository<T> repository)
        {
            Repository = repository;
        }

    }
}
