using Server.Infrastruct.Model.Dto.InDto;
using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Enums;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Repository;
using Server.Infrastruct.Services.Authentication;

namespace Server.Infrastruct.Services.DB
{
    public class EnableService<T> : AuthService<T>, IEnableService<T> where T : CommonEntity, IEnabled, new()
    {
        public EnableService(IBaseRepository<T> repository, IAuthenticationService authenticationService) : base(repository, authenticationService)
        {
        }

        public void SetEnabled(SetListEnabledInDto dto)
        {
            if (dto == null)
            {
                return;
            }

            //根据主键更新
            Repository.Update(f => new T { IsEnabled = dto.IsEnabled, UpdateTime = DateTime.Now, UpdateUser = userModel.UserId }, f => dto.IDs.Contains(f.ID));
        }


        /// <summary>
        /// 获取所有启用中的数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetEnableList()
        {
            return Repository.QueryListByCondition(f => f.IsEnabled == StatesEnum.Y);
        }

        /// <summary>
        /// 获取所有启用中的数据(分页)
        /// </summary>
        /// <returns></returns>
        public PageModel<T> GetEnablePageList(int pageIndex, int pageSize)
        {
            return Repository.QueryPageList(f => f.IsEnabled == StatesEnum.Y, f => f.UpdateTime, pageIndex, pageSize, SqlSugar.OrderByType.Desc);
        }
    }
}
