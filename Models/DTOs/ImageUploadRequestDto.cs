namespace StudentManagementSystem.Models.DTOs
{
    public class ImageUploadRequestDto
    {
        public IFormFile File { get; set; }

        public string? FileDescription { get; set; }

    }
}