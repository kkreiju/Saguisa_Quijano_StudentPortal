using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;

namespace StudentPortal.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Students> Student { get; set; }
    }
}
