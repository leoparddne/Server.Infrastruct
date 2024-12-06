using Server.Infrastruct.Model.Entity;

namespace Server.Infrastruct.Services.DB
{
    public interface IAuthService<T> : IAutoService<T> where T : CommonEntity, new()
    {
        new void Create(T entity,bool saveUserID=true);
        new void Create(List<T> entity, bool saveUserID = true);
        new void Update(List<T> entity, bool saveUserID = true);
        new void Update(T entity, bool saveUserID = true);
    }
}
