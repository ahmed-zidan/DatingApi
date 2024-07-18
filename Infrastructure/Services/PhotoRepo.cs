using Core;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PhotoRepo : IPhotoRepo
    {
        private readonly AppDbContext _appDbContext;
        public PhotoRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task addPhoto(Photo photo)
        {
            await _appDbContext.Photos.AddAsync(photo);
        }

        public async Task<List<Photo>> getAllPhotos(string userId)
        {
            return await _appDbContext.Photos.Where(x => x.userId == userId).ToListAsync();
        }

        public async Task<Photo> getMainPhoto(string userId)
        {
            return await _appDbContext.Photos.Where(x => x.userId == userId).FirstOrDefaultAsync();
        }
    }
}
