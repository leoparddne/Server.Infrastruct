using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.Authentication;

namespace Server.Infrastruct.Services.DB
{
    public class AuthService<T> : AutoService<T>, IAuthService<T> where T : CommonEntity, new()
    {
        private IAuthenticationService authenticationService;

        protected UserInfoModel userModel
        {
            get
            {
                return  authenticationService.GetUserInfo();
            }
        }
        public AuthService(IBaseRepository<T> repository, IAuthenticationService authenticationService) : base(repository)
        {
            this.authenticationService = authenticationService;
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
