using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models.Domains;

namespace StudentManagementSystem.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Images> Imagess { get; set; }
    }
}
