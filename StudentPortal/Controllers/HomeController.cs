using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Diagnostics;

namespace StudentPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDBContext DBContext;

		public HomeController(ILogger<HomeController> logger, ApplicationDBContext DBContext)
        {
            _logger = logger;
			this.DBContext = DBContext;
        }

		

		[HttpGet]
        public async Task<IActionResult> Index()
		{
			// Retrieve the list of tables
			var students = await DBContext.Student.ToListAsync();
			var subjects = await DBContext.Subject.ToListAsync();
			var schedules = await DBContext.Schedule.ToListAsync();

			// Create the view model with both data
			var viewModel = new IndexViewModel
			{
                Students = students,
				Schedules = schedules,
				Subjects = subjects
			};

			return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
