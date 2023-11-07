using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models.classes;

namespace Backend.Interfaces
{
    public interface IImageRepository
    {
        public Task<bool> UploadImage(IFormFile file, int userId, string imageTitle, string imageDescription);
        public Task<Image> GetImageById(int imageId);
        public Task<bool> Save();
        public Task<List<Image>> GetImagesByUserId(int userId);
        public Task<bool> UploadProfileImage(IFormFile file, int userId);
        public Task<List<Image>> GetImagesWithDateSort();
        // public Task<ImageFile> GetImageFileByUserId(int userId);
    }
}