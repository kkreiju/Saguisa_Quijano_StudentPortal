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
			modelBuilder.Entity<Subjects>().HasKey(x => new
			{
				x.SubjCode, x.SubjCourseCode
			});
<<<<<<< Updated upstream
			modelBuilder.Entity<SubjectsPreq>().HasKey(x => new
			{
				x.SPSubjCode,
				x.SPSubjPreCode
			});
=======
			modelBuilder.Entity<Schedules>();
>>>>>>> Stashed changes
		}

		public DbSet<Students> Student { get; set; }
		public DbSet<Subjects> Subject { get; set; }
<<<<<<< Updated upstream
		public DbSet<SubjectsPreq> SubjectPreq { get; set; }
=======
		public DbSet<Schedules> Schedule { get; set; }
>>>>>>> Stashed changes
	}
}
