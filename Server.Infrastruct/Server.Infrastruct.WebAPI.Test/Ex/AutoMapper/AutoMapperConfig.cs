using AutoMapper;

namespace Server.Infrastruct.WebAPI.Test.Ex.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMapping()
        {
            return new MapperConfiguration(c =>
            {
                c.AddProfile(new CustomizedProfile());
            });
        }
    }
}
