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
				// Transaction is used associated to IDENTITY INSERT query
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						// To write values in primary key StudID
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

						ModelState.Clear();
						ViewBag.Message = "Student added.";
						return View(new StudentsViewModel());

					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			}
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var students = await DBContext.Student.ToListAsync();

			return View(students);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int? idnumber)
		{
			if (idnumber == null)
			{
				// When there is no ID submitted (initial load), do nothing
				ViewBag.SearchPerformed = false; // No search was performed yet
				return View();
			}

			var id = await DBContext.Student.FindAsync(idnumber);

			if (id != null)
			{
				ViewBag.ID = idnumber;
				return View(id);
			}
			else
			{
				ViewBag.Search = true;
				ViewBag.Message = "ID Not Found.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Students viewModel, string idnumber1, string idnumber2)
		{
			var student = await DBContext.Student.FindAsync(Convert.ToInt32(idnumber1));

			if(idnumber1.Trim().ToUpper().Equals(idnumber2.Trim().ToUpper()) && student is not null)
			{
				student.StudLName = viewModel.StudLName.ToUpper();
				student.StudFName = viewModel.StudFName.ToUpper();
				student.StudMName = viewModel.StudMName.ToUpper();
				student.StudCourse = viewModel.StudCourse.ToUpper();
				student.StudYear = viewModel.StudYear;
				student.StudRemarks = viewModel.StudRemarks;

				DBContext.Student.Update(student);

				await DBContext.SaveChangesAsync();
			}
			else
			{
				// Transaction is used associated to IDENTITY INSERT query
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						// To write values in primary key StudID
						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Student ON");

						var newstudent = new Students
						{
							StudID = Convert.ToInt32(idnumber2),
							StudLName = viewModel.StudLName.ToUpper(),
							StudFName = viewModel.StudFName.ToUpper(),
							StudMName = viewModel.StudMName.ToUpper(),
							StudCourse = viewModel.StudCourse.ToUpper(),
							StudYear = viewModel.StudYear,
							StudRemarks = viewModel.StudRemarks,
							StudStatus = student.StudStatus
						};

						await DBContext.Student.AddAsync(newstudent);
						await DBContext.SaveChangesAsync();

						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Student OFF");

						await transaction.CommitAsync();

						// Define the SQL command with a parameter placeholder
						var sqlCommand = "DELETE FROM Student WHERE StudID = {0}";

						// Execute the command with the specified parameter value
						int affectedRows = await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, idnumber1);
					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			}

			return RedirectToAction("List", "Students");
		}

		[HttpGet]
		public async Task<IActionResult> Delete (int? idnumber1)
		{
			if (idnumber1 == null)
			{
				// When there is no ID submitted (initial load), do nothing
				ViewBag.SearchPerformed = false; // No search was performed yet
				return View();
			}

			var id = await DBContext.Student.FindAsync(idnumber1);

			if (id != null)
			{
				return View(id);
			}
			else
			{
				ViewBag.Search = true;
				ViewBag.Message = "ID Not Found.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Students viewModel, string idnumber2)
		{
            // Define the SQL command with a parameter placeholder
            var sqlCommand = "DELETE FROM Student WHERE StudID = {0}";

            // Execute the command with the specified parameter value
            int affectedRows = await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, idnumber2);

            if (affectedRows > 0)
            {
                return RedirectToAction("List", "Students");
            }
            else
            {
                return NotFound();
            }
		}
	}
}
