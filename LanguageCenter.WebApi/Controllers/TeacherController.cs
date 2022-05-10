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

        /// <summary>
        /// Get all teachers
        /// </summary>
        /// <param name="page">int value for current page</param>
        /// <returns>
        /// Returns object with:
        /// 1.Current page
        /// 2.Pages
        /// 3.Collection of teachers which contains:
        ///  3.1.Teacher id
        ///  3.2.Full name
        ///  3.3.Email
        ///  3.4.Is active
        ///  3.5.Collection of language names
        ///  3.6.Collection of course names
        /// </returns>
        [HttpGet]
        [Route("get-all-teachers")]
        public async Task<IActionResult> GetAllTeachersAsync(int page)
        {
            var teachers = await _teacherService.GetAllTeachers(page);

            return Ok(teachers);
        }

        /// <summary>
        /// Get all currently active teachers
        /// </summary>
        /// <returns>
        /// Returns collection of objects which contains:
        /// 1.Teacher name
        /// 2.Teacher description
        /// </returns>
        [HttpGet]
        [Route("active-teachers")]
        public async Task<IActionResult> GetAllActiveTeachers()
        {
            var activeTeachers = await _teacherService.GetAllActiveTeachers();

            return Ok(activeTeachers);
        }

        /// <summary>
        /// Get selected teacher info 
        /// </summary>
        /// <param name="id">Selected teacher id</param>
        /// <returns>
        /// Returns object representing teacher entity.
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Get all teachers id
        /// </summary>
        /// <returns>
        /// Returns collection of teachers ids (as strings)
        /// </returns>
        [HttpGet]
        [Route("get-teachers-id")]
        public async Task<IActionResult> GetTeachersIdsAsync()
        {
            var teacherIds = await _teacherService.GetTeachersId();

            return Ok(teacherIds);
        }


        /// <summary>
        /// Get teacher description
        /// </summary>
        /// <param name="id">Selected teacher id</param>
        /// <returns>
        /// Returns object with description, user id and teacher id.
        /// If teacher is not found returns object with (string)message
        /// </returns>
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

        /// <summary>
        /// Adds array of languages to selected teacher
        /// </summary>
        /// <param name="teacherId">Selected teacher id</param>
        /// <param name="langauges">Array of objects with string property name</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
        [HttpPost]
        [Route("add-languages-to-teacher")]
        public async Task<IActionResult> AddLanguagesToTeacherAsync(string teacherId, ICollection<LanguageName> langauges)
        {
            try
            {
                var langaugeNames = langauges?
                    .Select(l => l.Name.ToUpper())
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

        /// <summary>
        /// Removes all languages from teacher
        /// </summary>
        /// <param name="id">Teacher id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Makes inactive teacher active
        /// </summary>
        /// <param name="id">Selected teacher id</param>
        /// Returns json object which contains (string)message property.
        /// <returns></returns>
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

        /// <summary>
        /// Makes user a teacher
        /// </summary>
        /// <param name="userId">Selected user id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Makes active teacher inactive
        /// </summary>
        /// <param name="id">Teacher id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Edit teacher description as admin
        /// </summary>
        /// <param name="teacherId">Selected teacher id</param>
        /// <param name="description">Teacher's description</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Edits teacher description. Checks if current user is authorized to edit the description.
        /// </summary>
        /// <param name="teacherId">Teacher id</param>
        /// <param name="userId">Current User id</param>
        /// <param name="description">Teacher description</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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
