using AutoMapper;
using Core;
using DatingApi.DTOS;
using System;

namespace DatingApi.Helpers
{
    public class MyMapper:Profile
    {
        public MyMapper()
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(x => x.age, y => y.MapFrom(src => src.getAge()))
                .ForMember(x=>x.PhotoUrl , y => y.MapFrom<ImageResolver>());
            CreateMap<UserDto,AppUser>();
            CreateMap<Photo,PhotoDto>().ForMember(x => x.Url, y => y.MapFrom<PhotoImageResolver>()); ;
        }
    }
}
