using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models.Domains;

namespace StudentManagementSystem.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly StudentDbContext _dbContext;

        public ImageRepository(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add new image
        public async Task<Images> AddImageAsync(Images image)
        {
            image.Id = Guid.NewGuid();
            await _dbContext.Imagess.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }

        // Update an existing image
        public async Task<Images?> UpdateImageAsync(Guid id, Images updatedImage)
        {
            var image = await _dbContext.Imagess.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null)
            {
                return null;
            }

            image.FileDescription = updatedImage.FileDescription;
            image.FileExtension = updatedImage.FileExtension;
            image.FileSizeInBytes = updatedImage.FileSizeInBytes;
            image.FilePath = updatedImage.FilePath;

            await _dbContext.SaveChangesAsync();
            return image;
        }

        // Delete an image
        public async Task<bool> DeleteImageAsync(Guid id)
        {
            var image = await _dbContext.Imagess.FirstOrDefaultAsync(i => i.Id == id);
            if (image == null)
            {
                return false;
            }

            _dbContext.Imagess.Remove(image);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Get all images
        public async Task<IEnumerable<Images>> GetAllImagesAsync()
        {
            return await _dbContext.Imagess.ToListAsync();
        }

        // Get an image by ID
        public async Task<Images?> GetImageAsync(Guid id)
        {
            return await _dbContext.Imagess.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
