using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPhotoRepo
    {
        Task addPhoto(Photo photo);
        Task<Photo> getMainPhoto(string userId);
        Task<List<Photo>> getAllPhotos(string userId);
        
    }
}
