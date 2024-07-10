using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.UnitOfWork;

namespace Server.Infrastruct.WebAPI.ServiceExtension
{
    public class BaseService : BaseRepositoryExtension, IBaseService
    {
        public BaseService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
