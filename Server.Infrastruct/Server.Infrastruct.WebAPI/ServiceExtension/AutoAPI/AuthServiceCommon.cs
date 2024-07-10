using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.Authentication;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Function;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public class AuthServiceCommon<T> : AutoServiceCommon<T>, IAuthServiceCommon<T>, IAutoCondition, IAutoCreate, IAutoDelete, IAutoSelect, IAutoUpdate, IAutoServiceBase<T> where T : CommonEntity, ICommonID, IEnabled, new()
    {
        private IAuthenticationService authenticationService;
        UserInfoModel userModel;
        public AuthServiceCommon(IBaseRepository<T> repository, IAuthenticationService authenticationService) : base(repository)
        {
            this.authenticationService = authenticationService;

            userModel = authenticationService.GetUserInfo();
        }

        public new void Create(T entity)
        {
            entity.Create(userModel.UserNo);
            base.Create(entity);
        }

        public new void Update(List<T> entity)
        {
            foreach (var item in entity)
            {
                item.Update(userModel.UserNo);
            }
            base.Update(entity);
        }
    }
}
