using AutoMapper;
using Core;
using DatingApi.DTOS;

namespace DatingApi.Helpers
{
    public class PhotoImageResolver : IValueResolver<Photo, PhotoDto, String>
    {
        private readonly IConfiguration _configuration;
        public PhotoImageResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Photo source, PhotoDto destination, string destMember, ResolutionContext context)
        {
            var mainPhoto = source.Url;
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
