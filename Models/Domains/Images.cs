using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models.Domains
{
    public class Images
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile? FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }

    }
}
