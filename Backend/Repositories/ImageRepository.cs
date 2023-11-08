using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;

        public ImageRepository(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<Image> GetImageById(int imageId)
        {
            var image = await _context.Images
                    .Include(i => i.ImageFile)
                    .FirstOrDefaultAsync(i => i.Id == imageId);
            if (image is null)
            {
                throw new Exception($"Image:{imageId} not found");
            }
            return image;
        }

        public async Task<List<Image>> GetImagesByUserId(int userId)
        {
            var images = await _context.Images
                .Include(i => i.ImageFile)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.UploadDate)
                .ToListAsync();

            return images;
        }

        public async Task<List<Image>> GetImagesWithDateSort()
        {
            var images = await _context.Images
                    .Include(i => i.ImageFile)
                    .OrderByDescending(i => i.UploadDate)
                    .ToListAsync();

            return images;
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UploadImage(IFormFile file, int userId, string imageTitle, string imageDescription)
        {
            var user = await _userRepository.GetUserById(userId);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);

                // Convert the image to a base64-encoded string.
                byte[] bytes = ms.ToArray();
                string base64Image = Convert.ToBase64String(bytes);

                // Create an ImageFile instance with the base64-encoded content.
                var imageFile = new ImageFile
                {
                    FileName = file.FileName,
                    FileContentBase64 = bytes // Store the binary data directly
                };

                var imageToSave = new Image
                {
                    Description = imageDescription,
                    Title = imageTitle,
                    ImageFile = imageFile,
                    UploadDate = DateTime.Now,
                    User = user,
                    UserId = userId
                };

                _context.Images.Add(imageToSave);

            }
            return await Save();
        }

        public async Task<bool> UploadProfileImage(IFormFile file, int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);

                // Convert the image to a base64-encoded string.
                byte[] bytes = ms.ToArray();
                string base64Image = Convert.ToBase64String(bytes);

                user.FileName = file.FileName;
                user.FileContentBase64 = bytes;

            }
            return await Save();
        }
    }
}