using Server.Infrastruct.Model.Entity;

namespace Server.Infrastruct.Services.DB
{
    public interface IAuthService<T> : IAutoService<T> where T : CommonEntity, new()
    {
        new void Create(T entity);

        new void Update(List<T> entity);
    }
}
