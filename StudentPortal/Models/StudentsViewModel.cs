using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class StudentsViewModel
    {
		[Key]
		public int? StudID { get; set; }

		[StringLength(20)]
		public string StudLName { get; set; }

		[StringLength(20)]
		public string StudFName { get; set; }

		[StringLength(20)]
		public string StudMName { get; set; }

		[StringLength(10)]
		public string StudCourse { get; set; }

		public int? StudYear { get; set; }

		[StringLength(15)]
		public string StudRemarks { get; set; }

		[StringLength(2)]
		public string StudStatus { get; set; }
	}
}
