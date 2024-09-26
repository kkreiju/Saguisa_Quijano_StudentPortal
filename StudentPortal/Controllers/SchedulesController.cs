using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

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
		public async Task<IActionResult> Entry(SchedulesViewModel viewModel, string edpcode, string subjectcode)
		{

			// Search for the student using the provided Edp code
			var EDPCode = await DBContext.Schedule.FirstOrDefaultAsync(s => s.EDPCode.ToString() == edpcode);


			if (EDPCode != null)
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Schedule is already registered.";
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
						await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Schedule ON");

						var schedule = new Schedules
						{
							EDPCode = Convert.ToInt32(edpcode),
							SubjCode = subjectcode.ToUpper(),
							StartTime = viewModel.StartTime,
							EndTime = viewModel.EndTime,
							Days = viewModel.Days,
							Category = viewModel.Category,
							Room = viewModel.Room.ToUpper(),
							MaxSize = viewModel.MaxSize,
							ClassSize = 0,
							Status = "AC",
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
			var schedules = await DBContext.Schedule.ToListAsync();

			return View(schedules);
		}
	}
}
