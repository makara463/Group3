using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Group3_LIbraryManagement_AGAAPP.Models;

namespace Group3_LIbraryManagement_AGAAPP.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<Penalty> Penalties { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

    }
}




