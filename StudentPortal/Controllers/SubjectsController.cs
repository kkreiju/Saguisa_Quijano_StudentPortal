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
		public async Task<IActionResult> Entry(SubjectsViewModel viewModel, string subjectcode, string coursecode, string reqsubjectcode, string requisitetype)
		{
			var subjCode = await DBContext.Subject.FirstOrDefaultAsync(s => s.SubjCode.ToString() == subjectcode);
<<<<<<< Updated upstream
			var primarykey1 = "";
			var primarykey2 = "";

=======
			var primarykey1 = string.Empty;
			var primarykey2 = string.Empty;

			// If there are row/s in the database
>>>>>>> Stashed changes
			if (subjCode != null)
			{
				primarykey1 = subjCode.SubjCode;
				primarykey2 = subjCode.SubjCourseCode;
			}

			if (primarykey1.Equals(subjectcode) && primarykey2.Equals(coursecode))
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = subjectcode + " is already registered on course " + coursecode + ".";
				return View();
			}
			else if(subjectcode == null || viewModel.SubjDesc == null ||
				viewModel.SubjUnits == null || viewModel.SubjCurrCode == null)
			{
				if(reqsubjectcode == null)
				{
					return View();
				}
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					var subject = await DBContext.Subject.FirstOrDefaultAsync(s => s.SubjCode.ToString().ToUpper() == reqsubjectcode.ToUpper());

					if (subject != null)
					{
						ViewBag.SubjCode = subject.SubjCode;
						ViewBag.SubjDesc = subject.SubjDesc;
						ViewBag.SubjUnits = subject.SubjUnits;
					}

					var requisite = await DBContext.SubjectPreq.FirstOrDefaultAsync(s => s.SPSubjCode.ToString().ToUpper() == reqsubjectcode.ToUpper());

					if (requisite != null)
					{
						ViewBag.SubjCategory = requisite.SPSubjPreCode;
					}

					if (subject == null && requisite == null)
					{
						ViewBag.RequisiteInfo = "Requisite null.";
					}
					else
					{
						ViewBag.RequisiteInfo = "Requisite not null.";
					}

					return View(new SubjectsViewModel());
				}
			}
			else
			{
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						var subject = new Subjects
						{
							SubjCode = subjectcode.ToUpper(),
							SubjDesc = viewModel.SubjDesc.ToUpper(),
							SubjUnits = viewModel.SubjUnits,
							SubjRegOfrng = viewModel.SubjRegOfrng,
							SubjCategory = viewModel.SubjCategory,
							SubjStatus = "AC",
							SubjCourseCode = coursecode,
							SubjCurrCode = viewModel.SubjCurrCode,
							SubjRequisite = viewModel.SubjRequisite
						};

						await DBContext.Subject.AddAsync(subject);
						await DBContext.SaveChangesAsync();
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes


						await transaction.CommitAsync();

						ViewBag.Message = "Subject added.";

						CheckRequisite(subjectcode, reqsubjectcode, requisitetype);


						ModelState.Clear();
						return View(new SubjectsViewModel());
<<<<<<< Updated upstream
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return View(new SubjectsViewModel());
					}
				}
			}
		}

		private async void CheckRequisite(string subjectcode, string reqsubjectcode, string requisitetype)
		{
			if (reqsubjectcode != null && requisitetype != null)
			{
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{

						var subjectpreq = new SubjectsPreq
						{
							SPSubjCode = subjectcode,
							SPSubjPreCode = reqsubjectcode,
							SPSubjCategory = requisitetype
						};

						await DBContext.SubjectPreq.AddAsync(subjectpreq);
						await DBContext.SaveChangesAsync();


						await transaction.CommitAsync();

=======
>>>>>>> Stashed changes
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
			var subjects = await DBContext.Subject.ToListAsync();

			return View(subjects);
		}

        [HttpGet]
        public async Task<IActionResult> Edit(string subjectcode, string coursecode)
        {
            if (subjectcode == null && coursecode == null)
            {
                // When there is no ID submitted (initial load), do nothing
                ViewBag.SearchPerformed = false; // No search was performed yet
                return View();
            }

            var subjectandcoursecode = await DBContext.Subject
			   .Where(s => s.SubjCode == subjectcode && s.SubjCourseCode == coursecode)
			   .FirstOrDefaultAsync();

			if (subjectandcoursecode != null)
            {
                return View(subjectandcoursecode);
            }
            else
            {
                ViewBag.Search = true;
                ViewBag.Message = "Subject Not Found.";
                return View();
            }
        }

		[HttpPost]
		public async Task<IActionResult> Edit(Subjects viewModel, string subjectcode, string coursecode)
		{
			var subjectandcoursecode = await DBContext.Subject
			   .Where(s => s.SubjCode == subjectcode && s.SubjCourseCode == coursecode)
			   .FirstOrDefaultAsync();

			if (subjectandcoursecode is not null)
			{
				subjectandcoursecode.SubjDesc = viewModel.SubjDesc.ToUpper();
				subjectandcoursecode.SubjUnits = viewModel.SubjUnits;
				subjectandcoursecode.SubjRegOfrng = viewModel.SubjRegOfrng;
				subjectandcoursecode.SubjCategory = viewModel.SubjCategory;
				subjectandcoursecode.SubjRequisite = viewModel.SubjRequisite.ToUpper();

				DBContext.Subject.Update(subjectandcoursecode);

				await DBContext.SaveChangesAsync();
			}

			return RedirectToAction("List", "Subjects");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string subjectcode, string coursecode)
		{
			if (subjectcode == null && coursecode == null)
			{
				// When there is no ID submitted (initial load), do nothing
				ViewBag.SearchPerformed = false; // No search was performed yet
				return View();
			}

			var subjectandcoursecode = await DBContext.Subject
			   .Where(s => s.SubjCode == subjectcode && s.SubjCourseCode == coursecode)
			   .FirstOrDefaultAsync();

			if (subjectandcoursecode != null)
			{
				return View(subjectandcoursecode);
			}
			else
			{
				ViewBag.Search = true;
				ViewBag.Message = "Subject Not Found.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Subjects viewModel, string subjectcode, string coursecode)
		{
			// Define the SQL command with a parameter placeholder
			var sqlCommand = "DELETE FROM Subject WHERE SubjCode = {0} AND SubjCourseCode = {1}";

			// Execute the command with the specified parameter value
			int affectedRows = await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, subjectcode, coursecode);

			if (affectedRows > 0)
			{
				return RedirectToAction("List", "Subjects");
			}
			else
			{
				return NotFound();
			}
		}
	}
}
