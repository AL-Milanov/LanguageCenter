using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Route("all-courses")]
        public async Task<IActionResult> GetAllCoursesAsync()
        {
            var courses = await _courseService.GetAllAsync();

            return Ok(courses);
        }

        [HttpGet]
        [Route("all-active-courses")]
        public async Task<IActionResult> GetActiveCoursesAsync([FromQuery]int page)
        {
            var courses = await _courseService.GetAllActiveAsync(page);

            return Ok(courses);
        }

        [HttpGet]
        [Route("all-courses-by-language")]
        public async Task<IActionResult> GetCoursesByLanguageAsync([FromQuery] string languageName)
        {
            var courses = await _courseService.GetCoursesByLanguageAsync(languageName);

            return Ok(courses);
        }

        [HttpPost]
        [Route("add-course")]
        public async Task<IActionResult> AddCourseAsync(AddCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _courseService.AddAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Created("/addcourse", new { message = "Course successfully added!" });
        }

        [HttpPost]
        [Route("add-teacher-to-course")]
        public async Task<IActionResult> AddTeacherToCourseAsync(
            [FromQuery] string courseId,
            [FromQuery] string teacherId)
        {
            try
            {
                await _courseService.AddTeacherToCourse(courseId, teacherId);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });

            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = dbEx.Message });
            }

            return Ok(new { message = "Teacher added to course successfully" });
        }

        [HttpPost()]
        [Route("delete-course")]
        public async Task<IActionResult> DeleteCourseAsync([FromQuery] string id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Course deleted successfully." });
        }

        [HttpGet]
        [Route("get-course")]
        public async Task<IActionResult> GetCourseAsync([FromQuery] string id, [FromQuery] string userId)
        {
            try
            {
                var course = await _courseService.GetByIdAsync(id, userId);

                return Ok(course);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);

            }
        }

        [HttpGet]
        [Route("get-course-details")]
        public async Task<IActionResult> GetCourseDetailsAsync([FromQuery] string id)
        {
            try
            {
                var course = await _courseService.GetDetailsAsync(id);

                return Ok(course);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);

            }
        }

        [HttpGet]
        [Route("get-students-from-course")]
        public async Task<IActionResult> GetStudentsFromCourseAsync([FromQuery] string id)
        {
            try
            {
                var course = await _courseService.GetStudentsFromCourseAsync(id);
                return Ok(course);
            }
            catch (ArgumentException)
            {
                return NotFound(new { message = "Course is not found!"});
            }
        }

        [HttpPost]
        [Route("remove-teacher-from-course")]
        public async Task<IActionResult> RemoveTeacherFromCourseAsync([FromQuery] string id)
        {
            try
            {
                await _courseService.RemoveTeacherFromCourse(id);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = dbEx.Message });
            }

            return Ok(new { message = "Teacher removed from course successfully" });
        }

        [HttpPost]
        [Route("update-course")]
        public async Task<IActionResult> UpdateCourseAsync(EditCourseInfoVM model)
        {
            try
            {
                await _courseService.UpdateCourse(model);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.Message);
            }

            return Ok(new { message = "Course updated successfully." });
        }

        [HttpPost]
        [Route("remove-student-from-course")]
        public async Task<IActionResult> RemoveStudentFromCourse(string courseId, string userId)
        {
            try
            {
                await _courseService.RemoveStudentFromCourseAsync(courseId, userId);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(new { message = arEx.Message });
            }
            catch(DbUpdateException dbEx)
            {
                return BadRequest(new { message = dbEx.Message});
            }

            return Ok(new { message = "User is removed from the course." });
        }
    }
}
