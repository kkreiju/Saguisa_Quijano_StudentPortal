using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
	public class SubjectsController : Controller
	{
		private readonly ApplicationDBContext DBContext;

        public SubjectsController(ApplicationDBContext DBContext)
        {
			this.DBContext = DBContext;
        }

        [HttpGet]
		public IActionResult Entry()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Entry(SubjectsViewModel viewModel, string subjectcode, string coursecode)
		{
			var subjCode = await DBContext.Subject.FirstOrDefaultAsync(s => s.SubjCode.ToString() == subjectcode);
			var primarykey1 = subjCode.SubjCode;
			var primarykey2 = subjCode.SubjCourseCode;

			if (primarykey1.Equals(subjectcode) && primarykey2.Equals(coursecode))
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Subject Code is already registered.";
				return View();
			}
			else
			{
				//ViewBag.Message = subjectcode + " : " + coursecode + " : " + viewModel.SubjDesc + " : " + viewModel.SubjUnits + " : " + viewModel.SubjRegOfrng
				//+ " : " + viewModel.SubjCategory + " : " + viewModel.SubjCurrCode;
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						//await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Subject ON");

						var subject = new Subjects
						{
							SubjCode = subjectcode,
							SubjDesc = viewModel.SubjDesc,
							SubjUnits = viewModel.SubjUnits,
							SubjRegOfrng = viewModel.SubjRegOfrng,
							SubjCategory = viewModel.SubjCategory,
							SubjStatus = "AC",
							SubjCourseCode = coursecode,
							SubjCurrCode = viewModel.SubjCurrCode
						};

						await DBContext.Subject.AddAsync(subject);
						await DBContext.SaveChangesAsync();
						

						//await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Subject OFF");

						await transaction.CommitAsync();
						ViewBag.Message = "Subject added.";
						ModelState.Clear();
						return View(new SubjectsViewModel());

						//if (ModelState.IsValid)
						//{
						//	ModelState.Clear();

						//	return View(new SubjectsViewModel());
						//}

					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			}
		}
	}
}
