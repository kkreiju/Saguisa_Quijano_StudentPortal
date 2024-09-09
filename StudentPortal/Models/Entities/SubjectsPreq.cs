using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
	public class SubjectsPreq
	{
		[StringLength(15)]
		public string SPSubjCode { get; set; }

		[StringLength(15)]
		public string SPSubjPreCode { get; set; }

		[StringLength(2)]
		public string SPSubjCategory { get; set; }

	}
}
