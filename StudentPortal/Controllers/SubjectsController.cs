using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		public async Task<IActionResult> Entry(SubjectsViewModel viewModel, string subjectcode, string coursecode, string subjrequisite)
		{
			var subjCode = await DBContext.Subject.FirstOrDefaultAsync(s => s.SubjCode.ToString() == subjectcode);
			var primarykey1 = string.Empty;
			var primarykey2 = string.Empty;

			var requisite = await DBContext.Subject
			   .Where(s => s.SubjCode == subjrequisite && s.SubjCourseCode == coursecode)
			   .FirstOrDefaultAsync();

			// If there are row/s in the database
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
			else if (requisite is null && subjrequisite is not null)
			{
				ViewBag.Message = subjrequisite + " is not registered on subjects and must be on the same course.";
				return View();
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
							SubjRequisite = subjrequisite?.ToUpper()
						};

						await DBContext.Subject.AddAsync(subject);
						await DBContext.SaveChangesAsync();

						await transaction.CommitAsync();

						ViewBag.Message = "Subject added.";


						ModelState.Clear();
						return View(new SubjectsViewModel());
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return View(new SubjectsViewModel());
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
				ViewBag.Subject = subjectcode;
				ViewBag.Course = coursecode;
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
		public async Task<IActionResult> Edit(Subjects viewModel, string subject, string course, string subjectcode, string coursecode, string subjrequisite)
		{
			ViewBag.Subject = subject;
			ViewBag.Course = course;

			var subjectandcoursecode = await DBContext.Subject
			   .Where(s => s.SubjCode == subject && s.SubjCourseCode == course)
			   .FirstOrDefaultAsync();

			var requisite = await DBContext.Subject
			   .Where(s => s.SubjCode == subjrequisite && s.SubjCourseCode == coursecode)
			   .FirstOrDefaultAsync();

			if (requisite is null && subjrequisite is not null)
			{
				ViewBag.Message = subjrequisite + " is not registered on subjects and must be on the same course.";
				return View(viewModel);
			}

			if ((subject.Trim().ToUpper().Equals(subjectcode.Trim().ToUpper()) &&
				course.Trim().ToUpper().Equals(coursecode.ToUpper())) && subjectandcoursecode is not null)
			{
				subjectandcoursecode.SubjDesc = viewModel.SubjDesc.ToUpper();
				subjectandcoursecode.SubjUnits = viewModel.SubjUnits;
				subjectandcoursecode.SubjRegOfrng = viewModel.SubjRegOfrng;
				subjectandcoursecode.SubjCategory = viewModel.SubjCategory;
				subjectandcoursecode.SubjRequisite = viewModel.SubjRequisite?.ToUpper();

				DBContext.Subject.Update(subjectandcoursecode);

				await DBContext.SaveChangesAsync();
			}
			else
			{
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						var newsubject = new Subjects
						{
							SubjCode = subjectcode.ToUpper(),
							SubjDesc = viewModel.SubjDesc.ToUpper(),
							SubjUnits = viewModel.SubjUnits,
							SubjRegOfrng = viewModel.SubjRegOfrng,
							SubjCategory = viewModel.SubjCategory,
							SubjStatus = subjectandcoursecode.SubjStatus,
							SubjCourseCode = coursecode,
							SubjCurrCode = viewModel.SubjCurrCode,
							SubjRequisite = viewModel.SubjRequisite?.ToUpper()
						};

						await DBContext.Subject.AddAsync(newsubject);
						await DBContext.SaveChangesAsync();

						ViewBag.Message = "Subject added.";

						// Define the SQL command with a parameter placeholder
						var sqlCommand = "DELETE FROM Subject WHERE SubjCode = {0} AND SubjCourseCode = {1}";

						// Execute the command with the specified parameter value
						await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, subject, course);

						//Update affected rows from schedule if the Subject Code is Updated
						sqlCommand = "UPDATE Schedule SET SubjCode = {0} WHERE SubjCode = {1} AND Course = {2}";
						await DBContext.Database.ExecuteSqlRawAsync(sqlCommand, subjectcode, subject, course);

						await transaction.CommitAsync();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw;
					}
				}
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
