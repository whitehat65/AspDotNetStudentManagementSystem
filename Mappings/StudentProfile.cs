using AutoMapper;
using StudentManagementSystem.Models.Domains;
using StudentManagementSystem.Models.DTOs;

namespace StudentManagementSystem.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));

            CreateMap<AddStudentDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id as it will be generated
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}