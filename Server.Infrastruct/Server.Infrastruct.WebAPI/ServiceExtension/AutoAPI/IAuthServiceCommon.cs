using Server.Domain.Entity;
using Server.Domain.Entity.Base;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public interface IAuthServiceCommon<T> : IAutoServiceCommon<T> where T : CommonEntity, ICommonID, IEnabled, new()
    {
    }
}
