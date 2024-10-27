using Server.Infrastruct.Model.Dto.InDto;
using Server.Infrastruct.Model.Entity;
using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Model;

namespace Server.Infrastruct.Services.DB
{
    public interface IEnableService<T> : IAuthService<T> where T : CommonEntity, IEnabled, new()
    {
        List<T> GetEnableList();
        PageModel<T> GetEnablePageList(int pageIndex, int pageSize);
        void SetEnabled(SetListEnabledInDto dto);
    }
}
