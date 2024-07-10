using Server.Domain.Entity;
using Server.Domain.Entity.Base;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public interface IAutoServiceCommonID<T> : IAutoServiceCommon<T> where T : CommonEntity, IEnabled, ICommonID, new()
    {
        //public IBaseRepository<T> Repository { get; set; }
    }
}
