using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;

namespace StudentPortal.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Students>();
			modelBuilder.Entity<Subjects>().HasKey(s => new
			{
				s.SubjCode, s.SubjCourseCode
			});
			modelBuilder.Entity<Schedules>();
			modelBuilder.Entity<EnrollmentDetails>().HasKey(e => new
			{
				e.ID, e.EDPCode
			});
			modelBuilder.Entity<EnrollmentHeaders>();
		}

		public DbSet<Students> Student { get; set; }
		public DbSet<Subjects> Subject { get; set; }
		public DbSet<Schedules> Schedule { get; set; }
		public DbSet<EnrollmentHeaders> EnrollmentHeader { get; set; }
		public DbSet<EnrollmentDetails> EnrollmentDetail { get; set; }
	}
}
