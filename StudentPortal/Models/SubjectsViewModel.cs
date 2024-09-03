using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
	public class SubjectsViewModel
	{
		[StringLength(15)]
		public string SubjCode { get; set; }

		[StringLength(40)]
		public string SubjDesc { get; set; }

		public Single? SubjUnits { get; set; }

		public int? SubjRegOfrng { get; set; }

		[StringLength(3)]
		public string SubjCategory { get; set; }

		[StringLength(2)]
		public string SubjStatus { get; set; }

		[StringLength(5)]
		public string SubjCourseCode { get; set; }

		[StringLength(10)]
		public string SubjCurrCode { get; set; }
	}
}
