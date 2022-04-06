using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ILanguageService _languageService;

        public AdminController(
            ICourseService courseService,
            ILanguageService languageService)
        {
            _courseService = courseService;
            _languageService = languageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddCourse()
        {
            var languages = await _languageService.GetAllAsync();

            ViewBag.Languages = languages;

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
