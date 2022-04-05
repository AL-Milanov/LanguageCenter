using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> All()
        {
            var courses = await _courseService.GetAllAsync();

            return View(courses);
        }
    }
}
