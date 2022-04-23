using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.WebApplication.Helper;
using LanguageCenter.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        private HttpClient _client;

        public CourseController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }


        public async Task<IActionResult> AddCourse(string? message)
        {

            using HttpResponseMessage response = await _client
                .GetAsync("/Language/all-languages-as-selected-list");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var languagesSelect = JsonConvert.DeserializeObject<List<SelectListItem>>(result);
                ViewBag.Languages = languagesSelect;
            }

            ViewBag.Message = message;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseVM model)
        {

            using HttpResponseMessage response = await _client
                .PostAsJsonAsync("/Course/add-course", model);

            var result = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllCourses), new { message = message?.Message });
            }

            return RedirectToAction(nameof(AddCourse), new { message = message?.Message });
        }

        public async Task<IActionResult> AllCourses(string? message)
        {
            ViewBag.Message = message;

            HttpResponseMessage response = await _client.GetAsync("/Course/all-active-courses");

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

            HttpResponseMessage response = await _client.PostAsync(
                $"/Course/delete-course?id={id}", null);

            var result = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllCourses), new { message = message?.Message });
            }
            else
            {
                return RedirectToAction(nameof(AllCourses), new { message = message?.Message });
            }

        }

        public async Task<IActionResult> CourseDetails(string id, string? message)
        {
            ViewBag.Message = message;

            var courseResponse = await _client.GetAsync($"/Course/get-course?id={id}");

            var course = new GetCourseVM();

            if (courseResponse.IsSuccessStatusCode)
            {
                var result = await courseResponse.Content.ReadAsStringAsync();
                course = JsonConvert.DeserializeObject<GetCourseVM>(result);
            }

            var teacherResponse = await _client
                .GetAsync($"/Language/all-teachers-by-language?language={course?.LanguageName}");

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

            HttpResponseMessage response = await _client
                .PostAsync($"/Course/add-teacher-to-course?courseId={courseId}&teacherId={model.TeacherName}",
                null);

            var result = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(CourseDetails), new { id = courseId, message = message?.Message });
            }

            return RedirectToAction(nameof(CourseDetails), new { id = courseId, message = message?.Message });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTeacherFromCourse(string courseId)
        {

            HttpResponseMessage response = await _client
                .PostAsync($"/Course/remove-teacher-from-course?id={courseId}", null);

            var result = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(CourseDetails), new { id = courseId, message = message?.Message });
            }

            return RedirectToAction(nameof(CourseDetails), new { id = courseId, message = message?.Message });
        }

        public async Task<IActionResult> EditCourse(string id, string? message)
        {
            ViewBag.Message = message;

            HttpResponseMessage response = await _client
                .GetAsync($"/Course/get-course?id={id}");

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
                        course.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Title = course.Title
                };

                return View(editCourseVM);
            }

            return RedirectToAction(nameof(AllCourses));
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(EditCourseInfoVM model)
        {

            HttpResponseMessage response = await _client
                .PostAsJsonAsync($"/Course/update-course", model);

            var result = await response.Content.ReadAsStringAsync();

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(CourseDetails), new { id = model.Id, message = message?.Message });
            }

            return RedirectToAction(nameof(EditCourse), new { id = model.Id, message = message?.Message });
        }
    }
}
