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
        [Route("active-teachers")]
        public async Task<IActionResult> GetAllActiveTeachers()
        {
            var activeTeachers = await _teacherService.GetAllActiveTeachers();

            return Ok(activeTeachers);
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
                return NotFound(new { message = arEx.Message });
            }
        }

        [HttpGet]
        [Route("get-teacher-ids")]
        public async Task<IActionResult> GetTeachersIdsAsync()
        {
            var teacherIds = await _teacherService.GetTeachersId();

            return Ok(teacherIds);
        }

        [HttpGet]
        [Route("get-teacher-description")]
        public async Task<IActionResult> GetTeacherDescription(string id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherDescription(id);
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
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
                return Ok(new { message = $"Successfully added {string.Join(",", langaugeNames)} to teacher." });
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

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

                return NotFound(new { message = arEx.Message });
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok(new { message = "Successfully removed languages from teacher" });
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
                return NotFound(new { message = arEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Successfully made teacher active." });
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
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "New teacher added." });
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
                return NotFound(new { message = arEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Teacher successfully made inactive." });
        }

        [HttpPost]
        [Route("admin-edit-description")]
        public async Task<IActionResult> EditDescriptionAsync(string teacherId, string description)
        {
            try
            {
                await _teacherService.EditDescription(teacherId, description);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Description edited successfully" });
        }

        [HttpPost]
        [Route("edit-description")]
        public async Task<IActionResult> EditDescriptionAsync(string teacherId, string userId, string description)
        {
            try
            {
                await _teacherService.EditDescription(teacherId, userId, description);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });
            }
            catch (UnauthorizedAccessException uaEx)
            {
                return Unauthorized(new { message = uaEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Description edited successfully" });
        }
    }
}
