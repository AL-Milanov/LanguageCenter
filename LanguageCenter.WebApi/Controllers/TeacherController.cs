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
        [Route("/get-all-teachers")]
        public async Task<IActionResult> GetAllTeachersAsync()
        {
            var teachers = await _teacherService.GetAllTeachers();

            return Ok(teachers);
        }

        [HttpGet]
        [Route("/get-teacher/id")]
        public async Task<IActionResult> GetTeacherAsync(string id)
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
        [Route("/get-teacher-ids")]
        public async Task<IActionResult> GetTeachersIdsAsync()
        {
            var teacherIds = await _teacherService.GetTeachersId();

            return Ok(teacherIds);
        }


        [HttpPost]
        [Route("/add-languages-to-teacher/teacherId")]
        public async Task<IActionResult> AddLanguagesToTeacherAsync(string teacherId, ICollection<string> langaugeNames)
        {
            try
            {
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
        [Route("/make-teacher-active/id")]
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
        [Route("/make-teacher/userId")]
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
        [Route("/make-teacher-unactive/id")]
        public async Task<IActionResult> MakeUnactive(string id)
        {
            try
            {
                await _teacherService.MakeUnactive(id);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
