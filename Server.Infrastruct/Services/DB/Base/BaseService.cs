using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.DB.UnitOfWork;

namespace Server.Infrastruct.Services.DB.Base
{
    /// <summary>
    /// 不提供任何增删改查逻辑,只提供基础数据库连接能力
    /// 与仓储无关,需要仓储时需要手动创建仓储类
    /// </summary>
    public class BaseService : BaseRepositoryExtension, IBaseService
    {
        public BaseService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
