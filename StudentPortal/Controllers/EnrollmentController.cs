using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
	public class EnrollmentController : Controller
	{
		private readonly ApplicationDBContext DBContext;

		public EnrollmentController(ApplicationDBContext DBContext)
        {
            this.DBContext = DBContext;
        }

		[HttpGet]
        public IActionResult Entry()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Entry(string idnumber)
		{

			// Search for the student using the provided ID number
			var student = await DBContext.Student.FirstOrDefaultAsync(s => s.StudID.ToString() == idnumber);

			if (student == null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Student not found.";
				ViewBag.ID = idnumber;
				return View();
			}
			else
			{
				ViewBag.Message = "Student found.";
				ViewBag.ID = idnumber;

				// Initialize a list to store the table lists
				var schedules = await DBContext.Schedule.ToListAsync();
				var subjects = await DBContext.Subject.ToListAsync();
				var enrollmenth = await DBContext.EnrollmentHeader.ToListAsync();
				var enrollmentd = await DBContext.EnrollmentDetail.ToListAsync();

				// Create the view model with datas
				var viewModel = new EnrollmentViewModel
				{
					Students = student,
					Schedules = schedules,
					Subjects = subjects,
					EnrollmentHeaders = enrollmenth,
					EnrollmentDetails = enrollmentd,
				};

				return View(viewModel);
			}
		}
	}
}
