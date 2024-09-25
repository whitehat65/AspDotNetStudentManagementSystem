using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models.Domains;
using StudentManagementSystem.Models.DTOs;
using StudentManagementSystem.Repositories;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _env;

        public ImageController(IImageRepository imageRepository, IWebHostEnvironment env)
        {
            _imageRepository = imageRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAllImagesAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var imageDtos = images.Select(image => new GetImageDto
            {
                Id = image.Id,
                FileDescription = image.FileDescription,
                FileExtension = image.FileExtension,
                FileSizeInBytes = image.FileSizeInBytes,
                FilePath = $"{baseUrl}/{image.FilePath.Replace("\\", "/")}"
            }).ToList();

            return Ok(imageDtos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {
            var image = await _imageRepository.GetImageAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var imageDto = new GetImageDto
            {
                Id = image.Id,
                FileDescription = image.FileDescription,
                FileExtension = image.FileExtension,
                FileSizeInBytes = image.FileSizeInBytes,
                FilePath = $"{baseUrl}/{image.FilePath.Replace("\\", "/")}"
            };

            return Ok(imageDto);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto uploadDto)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(uploadDto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }

            var image = new Images
            {
                FileDescription = uploadDto.FileDescription,
                FileExtension = Path.GetExtension(uploadDto.File.FileName),
                FileSizeInBytes = uploadDto.File.Length,
                FilePath = Path.Combine("uploads", fileName)
            };

            var createdImage = await _imageRepository.AddImageAsync(image);
            return Ok(new { message = "Image uploaded successfully", imageId = createdImage.Id });
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateImage([FromRoute] Guid id, [FromBody] ImageUploadRequestDto uploadDto)
        {
            var existingImage = await _imageRepository.GetImageAsync(id);
            if (existingImage == null)
            {
                return NotFound();
            }

            if (uploadDto.File != null && uploadDto.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(uploadDto.File.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadDto.File.CopyToAsync(stream);
                }

                existingImage.FileExtension = Path.GetExtension(uploadDto.File.FileName);
                existingImage.FileSizeInBytes = uploadDto.File.Length;
                existingImage.FilePath = Path.Combine("uploads", fileName);
            }

            existingImage.FileDescription = uploadDto.FileDescription;

            var updatedImage = await _imageRepository.UpdateImageAsync(id, existingImage);

            if (updatedImage == null)
            {
                return NotFound();
            }

            return Ok(new { message = "Image updated successfully", updatedImage });
        }

        // Delete Image
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid id)
        {
            var result = await _imageRepository.DeleteImageAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Image deleted successfully" });
        }
    }
}