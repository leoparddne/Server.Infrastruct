using Common.Toolkit.Helper;
using Server.Domain.Dto.InDto;
using Server.Domain.Entity;
using Server.Domain.Entity.Base;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Function;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI
{
    public class AutoServiceCommonID<T> : AutoServiceCommon<T>, IAutoCondition, IAutoCreate, IAutoDelete, IAutoSelect, IAutoUpdate, IAutoServiceCommonID<T> where T : CommonEntity, IEnabled, ICommonID, new()
    {
        //public IBaseRepository<T> Repository { get; set; }

        public AutoServiceCommonID(IBaseRepository<T> repository) : base(repository)
        {
            //this.Repository = repository;
        }

        public new void Create(T entity)
        {
            //id生成
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = GUIDHelper.NewGuid;
            }
            //entity.Create(userModel.UserNo);
            base.Create(entity);
        }


        public void SetEnabled(SetListEnabledInDto dto)
        {
            if (dto == null)
            {
                return;
            }

            //根据主键更新
            SetEnabled(dto, f => dto.Ids.Contains(f.Id));
        }
    }
}
