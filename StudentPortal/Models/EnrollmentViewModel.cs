namespace StudentPortal.Models
{
	public class EnrollmentViewModel
	{
		public Entities.Students Students { get; set; }
		public List<Entities.Schedules> Schedules { get; set; }
		public List<Entities.Subjects> Subjects { get; set; }
		public List<Entities.EnrollmentDetails> EnrollmentDetails { get; set; }
		public List<Entities.EnrollmentHeaders> EnrollmentHeaders { get; set; }
	}
}
