using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.WebApplication.Helper;
using LanguageCenter.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private HttpClient _client;

        public TeacherController(HttpClient client)
        {

            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> AllTeachers(string? message, int page = 1)
        {
            ViewBag.Message = message;

            var response = await _client.GetAsync($"/Teacher/get-all-teachers?page={page}");

            var result = await response.Content.ReadAsStringAsync();

            var teachers = JsonConvert.DeserializeObject<TeacherResponse>(result);

            return View(teachers);
        }

        public async Task<IActionResult> AddTeacher(int page = 1)
        {
            var userResponse = await _client.GetAsync($"/User/get-all-users?id={User.GetId()}&page={page}");
            var userResult = await userResponse.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<UserResponse>(userResult);

            var teacherResponse = await _client.GetAsync("/Teacher/get-teachers-id");
            var teacherResult = await teacherResponse.Content.ReadAsStringAsync();

            var teachers = JsonConvert.DeserializeObject<ICollection<string>>(teacherResult);

            users?.Users?
                .Where(u => !teachers.Contains(u.Id))
                .ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> MakeTeacher(string id)
        {
            var response = await _client.PostAsync($"/Teacher/make-teacher?id={id}", null);

            var result = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });

            }
            else
            {
                return RedirectToAction(nameof(AddTeacher), new { message = message?.Message });
            }

        }

        public async Task<IActionResult> TeacherLanguages(string id, string? message)
        {
            ViewBag.Message = message;

            var teacherResponse = await _client.GetAsync($"/Teacher/get-teacher?id={id}");

            var languagesResponse = await _client.GetAsync("/Language/all-languages-as-selected-list");

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
            var response = await _client.GetAsync($"/Teacher/get-teacher?id={model.Id}");

            var message = new ResponseMessage();


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var teacher = JsonConvert.DeserializeObject<GetTeacherVM>(result);

                var removeLanguagesResponse = await _client
                    .PostAsync($"/Teacher/remove-all-languages-from-teacher?id={model.Id}", null);

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
                        .PostAsJsonAsync($"/Teacher/add-languages-to-teacher?teacherId={teacher?.Id}", languageJsonModel);

                    var languageResponse = await languagesResponse.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<ResponseMessage>(languageResponse);


                    if (languagesResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
                    }
                }

                var removeLanguagesResult = await removeLanguagesResponse.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<ResponseMessage>(removeLanguagesResult);

            }

            return RedirectToAction(nameof(TeacherLanguages), new { id = model.Id, message = message?.Message });
        }

        public async Task<IActionResult> MakeInactive(string id)
        {

            var response = await _client.PostAsync($"/Teacher/make-teacher-inactive?id={id}", null);

            var result = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
            }

            return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });

        }

        public async Task<IActionResult> MakeActive(string id)
        {
            var response = await _client.PostAsync($"/Teacher/make-teacher-active?id={id}", null);

            var result = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
            }

            return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
        }

        public async Task<IActionResult> EditDescription(string id)
        {
            var response = await _client.GetAsync($"Teacher/get-teacher-description?id={id}");

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var teacher = JsonConvert.DeserializeObject<TeacherDescriptionVM>(result);

                return View(teacher);
            }

            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
        }

        [HttpPost]
        public async Task<IActionResult> EditDescription(string id, string description)
        {
            var response = await _client.PostAsync($"/Teacher/admin-edit-description?teacherId={id}&description={description}", null);

            var result = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllTeachers), new { message = message?.Message });
            }

            return View(new { message = message?.Message });
        }
    }
}
