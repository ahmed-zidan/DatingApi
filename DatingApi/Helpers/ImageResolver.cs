using AutoMapper;
using AutoMapper.Execution;
using Core;
using DatingApi.DTOS;

namespace DatingApi.Helpers
{
    public class ImageResolver : IValueResolver<AppUser, UserDto, string>
    {
        private readonly IConfiguration _configuration;
        public ImageResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(AppUser source, UserDto destination, string destMember, ResolutionContext context)
        {
            var mainPhoto = source.Photos.FirstOrDefault(x => x.IsMain == true).Url;
            if (!string.IsNullOrEmpty(mainPhoto))
            {
                return _configuration["ApiUrl"] + mainPhoto;
            }
            else
            {
                return null;
            }
        }
    }
}
