namespace StudentManagementSystem.Models.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
