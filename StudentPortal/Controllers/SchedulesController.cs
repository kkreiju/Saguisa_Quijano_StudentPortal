using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using static System.Collections.Specialized.BitVector32;
using System.Reflection.Emit;
using System.Numerics;

namespace StudentPortal.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDBContext DBContext;

        public SchedulesController(ApplicationDBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet]
        public IActionResult Entry()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Entry(SchedulesViewModel viewModel, string edpcode, string subjectcode, string course)
		{

			// Search for the student using the provided Edp code
			var EDPCode = await DBContext.Schedule.FirstOrDefaultAsync(s => s.EDPCode.ToString() == edpcode);
            var subjectandcourse = await DBContext.Subject
               .Where(s => s.SubjCode == subjectcode && s.SubjCourseCode == course)
               .FirstOrDefaultAsync();

			if(subjectandcourse == null)
			{
                // Optionally return an error or notification to the user
                ViewBag.Message = "Please add subject code " + subjectcode.ToUpper() + " with its respective course before adding a schedule.";
                return View();
            }

            if (EDPCode != null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Schedule is already registered.";
				return View();
			}
			else if(viewModel.StartTime >= viewModel.EndTime)
			{
				ViewBag.Message = "Start Time is Invalid.";
				viewModel.SubjCode = subjectcode;
				return View(viewModel);
			}
			else
			{

				// Transaction is used associated to IDENTITY INSERT query
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						// To write values in primary key StudID
						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Schedule ON");

						var schedule = new Schedules
						{
							EDPCode = Convert.ToInt32(edpcode),
							SubjCode = subjectcode.ToUpper(),
							StartTime = viewModel.StartTime,
							EndTime = viewModel.EndTime,
							Days = viewModel.Days,
							Room = viewModel.Room.ToUpper(),
							MaxSize = viewModel.MaxSize,
							ClassSize = 0,
							Status = "AC",
							Course = viewModel.Course,
							Section = viewModel.Section.ToUpper(),
							SchoolYear = viewModel.SchoolYear.ToUpper()
						};

						await DBContext.Schedule.AddAsync(schedule);
						await DBContext.SaveChangesAsync();

						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Schedule OFF");

						await transaction.CommitAsync();

						ModelState.Clear();
						ViewBag.Message = "Schedule added.";
						return View(new SchedulesViewModel());

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
            // Retrieve the list of schedules
            var schedules = await DBContext.Schedule.ToListAsync();

            // Initialize a list to store the subjects
            var subjects = new List<Subjects>();

            // Loop through each schedule and retrieve the corresponding subject
            foreach (var schedule in schedules)
            {
				// Retrieves the subject table values of the corresponding schedule row
                var subject = await DBContext.Subject
                    .Where(s => s.SubjCode == schedule.SubjCode && s.SubjCourseCode == schedule.Course)
                    .FirstOrDefaultAsync();

                if (subject != null)
                {
                    subjects.Add(subject);
                }
            }

            // Create the view model with both data
            var viewModel = new ScheduleAndSubjectViewModel
            {
                Schedules = schedules,
                Subjects = subjects
            };

            return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int? edpcode)
		{
			if (edpcode == null)
			{
				// When there is no EDP Code (initial load), do nothing
				ViewBag.SearchPerformed = false; // No search was performed yet
				return View();
			}

			var edp = await DBContext.Schedule.FindAsync(edpcode);

			if (edp != null)
			{
				ViewBag.EDP = edpcode;
				return View(edp);
			}
			else
			{
				ViewBag.Search = true;
				ViewBag.Message = "EDP Code Not Found.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Schedules viewModel, int? edpc, int? edpcode, string subjectcode, string course)
		{
			ViewBag.EDP = edpc;
			var edp = await DBContext.Schedule.FindAsync(edpc);

			var subjectandcourse = await DBContext.Subject
			   .Where(s => s.SubjCode == subjectcode && s.SubjCourseCode == course)
			   .FirstOrDefaultAsync();

			if (subjectandcourse == null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Please add subject code " + subjectcode.ToUpper() + " with its respective course before editing a schedule.";
				return View(edp);
			}

			if (edpc == edpcode)
			{
				edp.SubjCode = subjectcode.ToUpper();
				edp.StartTime = viewModel.StartTime;
				edp.EndTime = viewModel.EndTime;
				edp.Days = viewModel.Days;
				edp.Room = viewModel.Room;
				edp.MaxSize = viewModel.MaxSize;
				edp.Course = viewModel.Course;
				edp.Section = viewModel.Section.ToUpper();
				edp.SchoolYear = viewModel.SchoolYear.ToUpper();

				DBContext.Schedule.Update(edp);

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
						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Schedule ON");

						var newschedule = new Schedules
						{
							EDPCode = Convert.ToInt32(edpcode),
							SubjCode = subjectcode.ToUpper(),
							StartTime = viewModel.StartTime,
							EndTime = viewModel.EndTime,
							Days = viewModel.Days,
							Room = viewModel.Room.ToUpper(),
							MaxSize = viewModel.MaxSize,
							ClassSize = edp.ClassSize,
							Status = edp.Status,
							Course = viewModel.Course,
							Section = viewModel.Section.ToUpper(),
							SchoolYear = viewModel.SchoolYear.ToUpper()
						};

						await DBContext.Schedule.AddAsync(newschedule);
						await DBContext.SaveChangesAsync();

						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Schedule OFF");

						await transaction.CommitAsync();

						// Define the SQL command with a parameter placeholder
						var sqlCommand = "DELETE FROM Schedule WHERE EDPCode = {0}";

						// Execute the command with the specified parameter value
						int affectedRows = await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, edpc);
					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			}

			return RedirectToAction("List", "Schedules");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? edpcode)
		{
			if (edpcode == null)
			{
				// When there is no EDP Code submitted (initial load), do nothing
				ViewBag.SearchPerformed = false; // No search was performed yet
				return View();
			}

			var edp = await DBContext.Schedule.FindAsync(edpcode);

			if (edp != null)
			{
				return View(edp);
			}
			else
			{
				ViewBag.Search = true;
				ViewBag.Message = "EDP Code Not Found.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Students viewModel, int? edpcode)
		{
			// Define the SQL command with a parameter placeholder
			var sqlCommand = "DELETE FROM Schedule WHERE EDPCode = {0}";

			// Execute the command with the specified parameter value
			int affectedRows = await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, edpcode);

			if (affectedRows > 0)
			{
				return RedirectToAction("List", "Schedules");
			}
			else
			{
				return NotFound();
			}
		}
	}
}
