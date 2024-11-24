using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.Authentication;

namespace Server.Infrastruct.Services.DB
{
    public class AuthService<T> : AutoService<T>, IAuthService<T> where T : CommonEntity, new()
    {
        protected IAuthenticationService authenticationService;

        protected UserInfoModel userModel
        {
            get
            {
                return authenticationService.GetUserInfo();
            }
        }
        public AuthService(IBaseRepository<T> repository, IAuthenticationService authenticationService) : base(repository)
        {
            this.authenticationService = authenticationService;
        }

        public new void Create(T entity, bool saveUserID = true)
        {
            var user = saveUserID ? userModel.UserId : userModel.UserNo;
            entity.Create(user);
            base.Create(entity);
        }

        public new void Update(List<T> entity, bool saveUserID = true)
        {
            var user = saveUserID ? userModel.UserId : userModel.UserNo;

            foreach (var item in entity)
            {
                item.Update(user);
            }
            base.Update(entity);
        }


        public new void Update(T entity, bool saveUserID = true)
        {
            var user = saveUserID ? userModel.UserId : userModel.UserNo;

            entity.Update(user);
            base.Update(entity);
        }
    }
}
