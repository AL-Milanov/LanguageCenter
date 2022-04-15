using LanguageCenter.Core.Models.CourseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        
        public CourseController()
        {

        }


        public async Task<IActionResult> AddCourse()
        {

            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync("https://localhost:7188/all-languages-as-selected-list");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var languagesSelect = JsonConvert.DeserializeObject<List<SelectListItem>>(result);
                ViewBag.Languages = languagesSelect;
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseVM model)
        {

            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client
                .PostAsJsonAsync("https://localhost:7188/add-course", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllCourses));
            }

            return RedirectToAction(nameof(AddCourse));
        }

        public async Task<IActionResult> AllCourses()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:7188/all-courses");

            var courses = new List<AllCourseVM>();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                courses = JsonConvert.DeserializeObject<List<AllCourseVM>>(result);

            }

            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(string id)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(
                $"https://localhost:7188/delete-course/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Course deleted successfully";
                return RedirectToAction(nameof(AllCourses));
            }
            else
            {
                ViewBag.Error = "Something happend, course is not deleted.";
                return RedirectToAction(nameof(AllCourses));
            }

        }

        public async Task<IActionResult> CourseDetails(string id)
        {
            HttpClient client = new HttpClient();

            var courseResponse = await client.GetAsync($"https://localhost:7188/get-course/{id}");

            var course = new GetCourseVM();

            if (courseResponse.IsSuccessStatusCode)
            {
                var result = await courseResponse.Content.ReadAsStringAsync();
                course = JsonConvert.DeserializeObject<GetCourseVM>(result);
            }

            var teacherResponse = await client.GetAsync($"https://localhost:7188/all-teachers-by-language/{course?.LanguageName}");

            var teachers = new List<SelectListItem>();

            if (teacherResponse.IsSuccessStatusCode)
            {
                var result = await teacherResponse.Content.ReadAsStringAsync();
                teachers = JsonConvert.DeserializeObject<List<SelectListItem>>(result);
            }

            ViewBag.Teachers = teachers;

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> AssignTeacher(string courseId, GetCourseVM model)
        {
            var teacherId = model.TeacherName;

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client
                .PostAsync($"https://localhost:7188/add-teacher-to-course/{courseId}/{teacherId}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(CourseDetails), new { id = courseId });
            }

            return RedirectToAction(nameof(CourseDetails), new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTeacherFromCourse(string courseId)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client
                .PostAsync($"https://localhost:7188/remove-teacher-from-course/{courseId}", null);

            return RedirectToAction(nameof(CourseDetails), new { id = courseId });
        }

        public async Task<IActionResult> EditCourse(string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client
                .GetAsync($"https://localhost:7188/get-course/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var course = JsonConvert.DeserializeObject<GetCourseVM>(result);

                var editCourseVM = new EditCourseInfoVM()
                {
                    Description = course.Description,
                    DurationInMonths = course.DurationInMonths,
                    Id = id,
                    Level = course.Level,
                    StartDate = DateTime.ParseExact(
                        course.StartDate, "dd/MM/yyyy" ,CultureInfo.InvariantCulture),
                    Title = course.Title
                };

                return View(editCourseVM);
            }

            return RedirectToAction(nameof(AllCourses));
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(EditCourseInfoVM model)
        {

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client
                .PostAsJsonAsync($"https://localhost:7188/update-course", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(CourseDetails), new { id = model.Id });
            }

            return RedirectToAction(nameof(EditCourse), new { id = model.Id });
        }
    }
}
