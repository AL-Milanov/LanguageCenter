using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        [Route("get-all-teachers")]
        public async Task<IActionResult> GetAllTeachersAsync()
        {
            var teachers = await _teacherService.GetAllTeachers();

            return Ok(teachers);
        }

        [HttpGet]
        [Route("get-teacher")]
        public async Task<IActionResult> GetTeacherAsync([FromQuery] string id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacher(id);
                return Ok(teacher);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
        }

        [HttpGet]
        [Route("get-teacher-ids")]
        public async Task<IActionResult> GetTeachersIdsAsync()
        {
            var teacherIds = await _teacherService.GetTeachersId();

            return Ok(teacherIds);
        }


        [HttpPost]
        [Route("add-languages-to-teacher")]
        public async Task<IActionResult> AddLanguagesToTeacherAsync(string teacherId, ICollection<LanguageName> langauges)
        {
            try
            {
                var langaugeNames = langauges?
                    .Select(l => l.Name)
                    .ToList();

                await _teacherService.AddLanguagesToTeacher(teacherId, langaugeNames);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("remove-all-languages-from-teacher")]
        public async Task<IActionResult> RemoveLanguagesFromTeacherAsync(string id)
        {
            try
            {

                await _teacherService.RemoveLanguagesFromTeacher(id);
            }
            catch (ArgumentException arEx)
            {

                return NotFound(arEx.Message);
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("make-teacher-active")]
        public async Task<IActionResult> MakeActive(string id)
        {
            try
            {
                await _teacherService.MakeActive(id);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("make-teacher/userId")]
        public async Task<IActionResult> MakeTeacher(string userId)
        {
            try
            {
                await _teacherService.MakeTeacher(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("make-teacher-inactive")]
        public async Task<IActionResult> MakeInactive(string id)
        {
            try
            {
                await _teacherService.MakeInactive(id);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("active-teachers")]
        public async Task<IActionResult> GetAllActiveTeachers()
        {
            var activeTeachers = await _teacherService.GetAllActiveTeachers();

            return Ok(activeTeachers);
        }
    }
}
