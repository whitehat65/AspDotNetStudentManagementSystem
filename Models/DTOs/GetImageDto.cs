namespace StudentManagementSystem.Models.DTOs
{
    public class GetImageDto
    {
        public Guid Id { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }
    }
}
