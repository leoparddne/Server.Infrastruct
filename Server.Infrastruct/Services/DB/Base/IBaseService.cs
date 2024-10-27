using Server.Infrastruct.Repository;

namespace Server.Infrastruct.Services.DB.Base
{
    /// <summary>
    /// 不提供任何增删改查逻辑,只提供基础数据库连接能力
    /// </summary>
    public interface IBaseService : IBaseRepositoryExtension
    {
    }
}
