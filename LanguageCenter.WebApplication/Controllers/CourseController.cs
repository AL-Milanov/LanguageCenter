using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.WebApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanguageCenter.WebApplication.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient _client;

        public CourseController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> All()
        {
            var response = await _client.GetAsync("/Course/all-active-courses");

            var result = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<List<AllCourseVM>>(result);

            return View(courses);
        }

        public async Task<IActionResult> AllCoursesByLanguage([FromQuery] string language)
        {
            var response = await _client.GetAsync($"/Course/all-courses-by-language?languageName={language}");

            var result = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<List<AllCourseVM>>(result);

            return View(courses);
        }

        public async Task<IActionResult> GetCourse(string id)
        {

            return View();
        }
    }
}
