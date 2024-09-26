using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
	public class Schedules
	{
		[Key]
		public int? EDPCode { get; set; }

		[StringLength(15)]
		public string SubjCode { get; set; }

		public TimeOnly StartTime { get; set; }

		public TimeOnly EndTime { get; set; }

		public string Days { get; set; }

		[StringLength(3)]
		public string Category { get; set; }

		[StringLength(5)]
		public string Room { get; set; }

		public int? MaxSize { get; set; }

		public int? ClassSize { get; set; }

		[StringLength(3)]
		public string Status { get; set; }

		[StringLength(3)]
		public string Section { get; set; }

		[StringLength(10)]
		public string SchoolYear { get; set; }
	}
}
