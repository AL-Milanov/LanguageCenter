using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        private readonly IUserService _userService;

        private readonly ILanguageService _languageService;

        public TeacherController(
            ITeacherService teacherService,
            IUserService userService,
            ILanguageService languageService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _languageService = languageService;

        }

        public async Task<IActionResult> AllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachers();

            return View(teachers);
        }

        public async Task<IActionResult> AddTeacher()
        {
            var users = await _userService
                .GetAll(u => u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            var teachers = await _teacherService
                .GetTeachersId();


            var usersNotTeacher = users
                .Where(u => !teachers.Contains(u.Id))
                .ToList();

            return View(usersNotTeacher);
        }

        [HttpPost]
        public async Task<IActionResult> MakeTeacher(string id)
        {
            var result = await _teacherService.MakeTeacher(id);

            if (!result)
            {
                return RedirectToAction(nameof(AddTeacher));
            }

            return RedirectToAction(nameof(AllTeachers));
        }

        public async Task<IActionResult> TeacherLanguages(string id)
        {
            var teacher = await _teacherService.GetTeacher(id);

            ViewBag.Languages = await _languageService.GetAllAsSelectListAsync();

            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> TeacherLanguages(GetTeacherVM model)
        {
            var teacher = await _teacherService.GetTeacher(model.Id);

            await _teacherService.RemoveLanguagesFromTeacher(model.Id);

            if (model.Languages?.Count > 0)
            {
                await _teacherService.AddLanguagesToTeacher(teacher.Id, model.Languages);
            }

            return RedirectToAction(nameof(AllTeachers));
        }
    }
}
