using AutoMapper;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.DTOs;

namespace IdentityAuthWithJWT
{
	public class MapperInitializer : Profile
	{
        public MapperInitializer()
        {
            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
