using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Runtime.CompilerServices;

namespace StudentPortal.Controllers
{
	public class StudentsController : Controller
	{
		private readonly ApplicationDBContext DBContext;

		public StudentsController(ApplicationDBContext DBContext)
		{
			this.DBContext = DBContext;
		}

		[HttpGet]
		public IActionResult Entry()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Entry(StudentsViewModel viewModel, string idnumber)
		{

			// Search for the student using the provided ID number
			var studIdNumber = await DBContext.Student.FirstOrDefaultAsync(s => s.StudID.ToString() == idnumber);

			if (studIdNumber != null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Student is already registered.";
				return View();
			}
			else
			{
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Student ON");

						var student = new Students
						{
							StudID = Convert.ToInt32(idnumber),
							StudLName = viewModel.StudLName.ToUpper(),
							StudFName = viewModel.StudFName.ToUpper(),
							StudMName = viewModel.StudMName.ToUpper(),
							StudCourse = viewModel.StudCourse.ToUpper(),
							StudYear = viewModel.StudYear,
							StudRemarks = viewModel.StudRemarks,
							StudStatus = "AC"
						};

						await DBContext.Student.AddAsync(student);
						await DBContext.SaveChangesAsync();

						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Student OFF");

						await transaction.CommitAsync();

						if (ModelState.IsValid)
						{
							ModelState.Clear();
							ViewBag.Message = "Student added.";
							return View(new StudentsViewModel());
						}

					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			}

			return View();
		}
	}
}
