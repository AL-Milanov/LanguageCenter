using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.WebApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly IUserService _userService;

        private HttpClient _client;

        public TeacherController(
            IUserService userService,
            HttpClient client)
        {
            _userService = userService;

            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> AllTeachers()
        {
            var response = await _client.GetAsync("get-all-teachers");

            var result = await response.Content.ReadAsStringAsync();

            var teachers = JsonConvert.DeserializeObject<IEnumerable<GetAllTeachersVM>>(result);

            return View(teachers);
        }

        public async Task<IActionResult> AddTeacher()
        {
            var users = await _userService
                .GetAll(u => u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await _client.GetAsync("/get-teacher-ids");

            var result = await response.Content.ReadAsStringAsync();

            var teachers = JsonConvert.DeserializeObject<ICollection<string>>(result);

            var usersNotTeacher = users
                .Where(u => !teachers.Contains(u.Id))
                .ToList();

            return View(usersNotTeacher);
        }

        [HttpPost]
        public async Task<IActionResult> MakeTeacher(string id)
        {
            var response = await _client.PostAsync($"/make-teacher/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers));

            }
            else
            {
                return RedirectToAction(nameof(AddTeacher));
            }

        }

        public async Task<IActionResult> TeacherLanguages(string id)
        {
            var teacherResponse = await _client.GetAsync($"/get-teacher?id={id}");

            var languagesResponse = await _client.GetAsync("/all-languages-as-selected-list");

            if (teacherResponse.IsSuccessStatusCode && languagesResponse.IsSuccessStatusCode)
            {
                var teacherResult = await teacherResponse.Content.ReadAsStringAsync();

                var teacher = JsonConvert.DeserializeObject<GetTeacherVM>(teacherResult); 

                var languagesResult = await languagesResponse.Content.ReadAsStringAsync();

                var languages = JsonConvert.DeserializeObject<List<SelectListItem>>(languagesResult);

                ViewBag.Languages = languages;

                return View(teacher);
            }

            return RedirectToAction(nameof(AllTeachers));
        }

        [HttpPost]
        public async Task<IActionResult> TeacherLanguages(GetTeacherVM model)
        {
            var response = await _client.GetAsync($"/get-teacher?id={model.Id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var teacher = JsonConvert.DeserializeObject<GetTeacherVM>(result);

                var removeLanguagesResponse = await _client
                    .PostAsync($"/remove-all-languages-from-teacher?id={model.Id}", null);

                if (removeLanguagesResponse.IsSuccessStatusCode)
                {
                    var languageJsonModel = new List<LanguageName>();

                    if (model.Languages != null)
                    {

                        languageJsonModel = model.Languages?
                            .Select(l => new LanguageName
                            {
                                Name = l
                            })
                            .ToList();
                    }

                    var languagesResponse = await _client
                        .PostAsJsonAsync($"/add-languages-to-teacher?teacherId={teacher?.Id}", languageJsonModel);

                    if (languagesResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(AllTeachers));
                    }
                }

            }

            return RedirectToAction(nameof(TeacherLanguages), new { id = model.Id });
        }

        public async Task<IActionResult> MakeInactive(string id)
        {

            var response = await _client.PostAsync($"/make-teacher-inactive?id={id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers));
            }

            //need modifications
            return RedirectToAction(nameof(AllTeachers));

        }

        public async Task<IActionResult> MakeActive(string id)
        {
            var response = await _client.PostAsync($"/make-teacher-active?id={id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers));
            }

            //need modifications
            return RedirectToAction(nameof(AllTeachers));
        }
    }
}
