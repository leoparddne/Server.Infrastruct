using Server.Infrastruct.WebAPI.Repository;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public interface IAutoServiceBase<T> where T : class
    {
        public IBaseRepository<T> Repository { get; set; }
    }
}
