using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Reflection.Emit;

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

		[HttpPost]
		public async Task<IActionResult> EnrollStudent([FromBody] List<EnrollmentDetails> data, string idnumber, int units)
		{
			// Search for the student using the provided ID number
			var subjectcode = await DBContext.Schedule.FirstOrDefaultAsync(s => s.EDPCode.ToString() == 0.ToString()); // Sync this in as dummy data
			var sqlCommand = "";

			// Transaction is used associated to IDENTITY INSERT query
			using (var transaction = await DBContext.Database.BeginTransactionAsync())
			{
				foreach (var item in data)
				{
					try
					{
						subjectcode = await DBContext.Schedule.FirstOrDefaultAsync(s => s.EDPCode.ToString() == item.EDPCode.ToString());

						// Define the SQL command with a parameter placeholder
						sqlCommand = "INSERT INTO EnrollmentDetail VALUES ({0}, {1}, {2})";

						// Execute the command with the specified parameter value
						await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, Convert.ToInt32(idnumber), item.EDPCode, subjectcode.SubjCode);

						sqlCommand = "UPDATE Schedule SET ClassSize = {0} WHERE EDPCode = {1}";
						await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, subjectcode.ClassSize + 1, subjectcode.EDPCode);

						if(subjectcode.ClassSize + 1 == subjectcode.MaxSize)
						{
							sqlCommand = "UPDATE Schedule SET Status = {0} WHERE EDPCode = {1}";
							await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, "IN", subjectcode.EDPCode);
						}
					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}

				await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT EnrollmentHeader ON");

				sqlCommand = "INSERT INTO EnrollmentHeader (ID, DateEnroll, SchoolYear, TotalUnits, Status) VALUES ({0}, {1}, {2}, {3}, {4})";
				await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, Convert.ToInt32(idnumber), DateOnly.FromDateTime(DateTime.UtcNow), "A.Y. 2024-2025", units, "AC");

				await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT EnrollmentHeader OFF");

				await transaction.CommitAsync();
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult StudyLoad()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> StudyLoad(string idnumber)
		{
			// Search for the student using the provided ID number
			var studentenrolled = await DBContext.EnrollmentHeader.FirstOrDefaultAsync(s => s.ID.ToString() == idnumber);
			var student = await DBContext.Student.FirstOrDefaultAsync(s => s.StudID.ToString() == idnumber);

			if (studentenrolled == null && student != null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Student not enrolled.";
				ViewBag.ID = idnumber;
				return View();
			}
			else if(studentenrolled == null && student == null)
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
