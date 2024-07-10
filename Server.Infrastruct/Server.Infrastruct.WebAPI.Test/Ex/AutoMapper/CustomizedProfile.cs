using Consul;
using Server.Domain.Dto.OutDto;
using AutoMapper;

namespace Server.Infrastruct.WebAPI.Test.Ex.AutoMapper
{
    public class CustomizedProfile : Profile
    {
        public CustomizedProfile()
        {
            #region Select
            //CreateMap<LanguageEntity, SelectOutDto>()
            //    .ForMember(dest => dest.Value, opt => opt.MapFrom(t => t.LanguageNo))
            //    .ForMember(dest => dest.Label, opt => opt.MapFrom(t => t.LanguageName));
            #endregion
        }
    }
}
