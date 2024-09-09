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
			var primarykey1 = "";
			var primarykey2 = "";

			if (subjCode != null)
			{
				primarykey1 = subjCode.SubjCode;
				primarykey2 = subjCode.SubjCourseCode;
			}

			if (primarykey1.Equals(subjectcode) && primarykey2.Equals(coursecode))
			{
				// Optionally return an error or notification to the user
				ViewBag.Message = "Subject Code is already registered.";
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
				//ViewBag.Message = subjectcode + " : " + coursecode + " : " + viewModel.SubjDesc + " : " + viewModel.SubjUnits + " : " + viewModel.SubjRegOfrng
				//+ " : " + viewModel.SubjCategory + " : " + viewModel.SubjCurrCode;
				using (var transaction = await DBContext.Database.BeginTransactionAsync())
				{
					try
					{
						//await DBContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT Subject ON");

						var subject = new Subjects
						{
							SubjCode = subjectcode.ToUpper(),
							SubjDesc = viewModel.SubjDesc.ToUpper(),
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

						CheckRequisite(subjectcode, reqsubjectcode, requisitetype);


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
