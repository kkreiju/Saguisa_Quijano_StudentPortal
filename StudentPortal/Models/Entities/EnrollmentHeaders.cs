using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
	public class EnrollmentHeaders
	{
		[Key]
		public int ID { get; set; }

		public DateOnly DateEnroll { get; set; }

		public string SchoolYear { get; set; }

		public int TotalUnits { get; set; }

		[StringLength(2)]
		public string Status { get; set; }
	}
}
