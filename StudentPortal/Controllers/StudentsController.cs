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
		public async Task<IActionResult> Entry(StudentsViewModel viewModel)
		{
			using(var transaction = await DBContext.Database.BeginTransactionAsync())
			{
				try
				{
					await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Student ON");

					var student = new Students
					{
						StudID = viewModel.StudID,
						StudLName = viewModel.StudLName,
						StudFName = viewModel.StudFName,
						StudMName = viewModel.StudMName,
						StudCourse = viewModel.StudCourse,
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
						return View(new StudentsViewModel());
					}

				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}

			return View();
		}
	}
}
