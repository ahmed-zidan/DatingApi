using AutoMapper;
using Core;
using Core.Interfaces;
using DatingApi.DTOS;
using DatingApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DatingApi.Controllers
{
    
    public class PhotoController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public PhotoController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _mapper = mapper;
            _uow = unitOfWork; 
           
        }

        [HttpPost("UploadMainPhoto"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMainPhoto()
        {
            var userId = HttpContext.getCurrentUserId();
            var mainPhoto = await _uow._photo.getMainPhoto(userId);
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                if (mainPhoto != null)
                {
                    var url = mainPhoto.Url;
                    mainPhoto.Url = dbPath;
                    System.IO.File.Delete(url);
                    
                }
                else
                {
                    await _uow._photo.addPhoto(new Photo()
                    {
                        IsMain = true,
                        PublicId = 1,
                        Url = dbPath,
                        userId = userId,
                    });
                }
                await _uow.SaveChangesAsync();
                return Ok(new { dbPath });
            }
            else
            {
                return BadRequest();
            }


        }

    }
}
