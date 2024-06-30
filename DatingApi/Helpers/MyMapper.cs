using AutoMapper;
using Core;
using DatingApi.DTOS;

namespace DatingApi.Helpers
{
    public class MyMapper:Profile
    {
        public MyMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
