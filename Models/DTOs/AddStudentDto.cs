namespace StudentManagementSystem.Models.DTOs
{
    public class AddStudentDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
