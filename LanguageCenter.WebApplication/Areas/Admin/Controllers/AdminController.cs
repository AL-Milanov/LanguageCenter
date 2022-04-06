using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ICourseService _courseService;

        public AdminController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCourse()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseVM model)
        {
            await _courseService.AddAsync(model);

            return View(nameof(Index));
        }

        public async Task<IActionResult> AllCourses()
        {
            var courses = await _courseService.GetAllAsync();

            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await _courseService.DeleteAsync(id);

            if (!result)
            {
                ViewBag.Error = "Something happend course is not deleted.";
                return View();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
