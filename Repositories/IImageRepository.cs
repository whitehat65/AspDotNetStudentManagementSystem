using StudentManagementSystem.Models.Domains;

namespace StudentManagementSystem.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Images>> GetAllImagesAsync();
        Task<Images?> GetImageAsync(Guid id);
        Task<Images> AddImageAsync(Images image);
        Task<Images?> UpdateImageAsync(Guid id, Images image);

        Task<bool> DeleteImageAsync(Guid id);
    }
}
