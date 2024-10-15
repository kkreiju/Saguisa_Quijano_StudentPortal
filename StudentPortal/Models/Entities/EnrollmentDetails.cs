using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
	public class EnrollmentDetails
	{
		public int ID { get; set; }

		public string SubjCode { get; set; }

		public int EDPCode { get; set; }
	}
}
