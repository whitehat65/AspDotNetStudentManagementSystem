using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.Domains;
using StudentManagementSystem.Models.DTOs;
using StudentManagementSystem.Repositories;
using System.Linq.Dynamic.Core;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllStudents([FromQuery] PaginationFilter paginationFilter)
        {
            var students = await _studentRepository.GetAllStudentsAsync();

            // Filter
            if (!string.IsNullOrEmpty(paginationFilter.Filter))
            {
                students = students
                    .Where(s => s.Name.Contains(paginationFilter.Filter, StringComparison.OrdinalIgnoreCase) ||
                                s.ProfileImageUrl.Contains(paginationFilter.Filter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Sorting
            if (!string.IsNullOrEmpty(paginationFilter.SortBy))
            {
                var sortDirection = paginationFilter.SortDirection.ToLower() == "desc" ? "descending" : "ascending";
                students = students.AsQueryable().OrderBy($"{paginationFilter.SortBy} {sortDirection}").ToList();
            }

            // Pagination
            var totalRecords = students.Count();
            var studentsPaged = students.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                         .Take(paginationFilter.PageSize)
                                         .ToList();

            var studentDto = _mapper.Map<List<StudentDto>>(studentsPaged);
            return Ok(new
            {
                TotalRecords = totalRecords,
                Students = studentDto
            });
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }

        [HttpPost]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest("Student data is null.");
            }

            var student = _mapper.Map<Student>(studentDto);
            student.Id = Guid.NewGuid(); // Assign a new ID

            await _studentRepository.AddStudentAsync(student);

            var newStudentDto = _mapper.Map<StudentDto>(student);
            return Ok(new { message = "Student added successfully", newStudentDto });
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, [FromBody] AddStudentDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest("Student data is null.");
            }

            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Update properties
            _mapper.Map(studentDto, student);
            await _studentRepository.UpdateStudentAsync(student);

            return Ok(new { message = "Student updated successfully", studentDto });
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            await _studentRepository.DeleteStudentAsync(id);
            return Ok(new { message = "Student deleted successfully", student });
        }
    }
}