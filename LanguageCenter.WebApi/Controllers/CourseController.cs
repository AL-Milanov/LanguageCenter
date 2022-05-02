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

        /// <summary>
        /// Returns all courses.
        /// </summary>
        /// <returns>Return collection of json objects representing course entity.
        /// Return model contains: id, title, level, languageName and startDate as string
        /// </returns>
        [HttpGet]
        [Route("all-courses")]
        public async Task<IActionResult> GetAllCoursesAsync()
        {
            var courses = await _courseService.GetAllAsync();

            return Ok(courses);
        }


        /// <summary>
        /// Returns all currently active courses.
        /// </summary>
        /// <param name="page">Number of page</param>
        /// <returns>Return json object.
        /// Return model contains:
        /// 1.Collection of course model with: id, title, level, languageName and startDate as string
        /// 2.CurrentPage - int representing current page
        /// 3.Pages  - int representing number of available pages
        /// </returns>
        [HttpGet]
        [Route("all-active-courses")]
        public async Task<IActionResult> GetActiveCoursesAsync([FromQuery] int page)
        {
            var courses = await _courseService.GetAllActiveAsync(page);

            return Ok(courses);
        }

        /// <summary>
        /// Returns all courses by specific language.
        /// </summary>
        /// <param name="languageName">Parameter for search query</param>
        /// <returns>Return collection of json objects representing course from specific language entity.
        /// Return model contains: id, title, level, languageName and startDate as string
        /// </returns>
        [HttpGet]
        [Route("all-courses-by-language")]
        public async Task<IActionResult> GetCoursesByLanguageAsync([FromQuery] string languageName)
        {
            var courses = await _courseService.GetCoursesByLanguageAsync(languageName);

            return Ok(courses);
        }

        /// <summary>
        /// Add course to collection.
        /// </summary>
        /// <param name="model">Object that contains:
        /// title, level, durationInMonths, description, startDate, languageName and (not required)teacherId 
        /// </param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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


        /// <summary>
        /// Asigns teacher to course
        /// </summary>
        /// <param name="courseId">Selected course</param>
        /// <param name="teacherId">Selected teacher</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Deletes certain course.
        /// </summary>
        /// <param name="id">Selected course id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Get specific information about selected course for users.
        /// </summary>
        /// <param name="id" string>Selected course id</param>
        /// <param name="userId">Current user id</param>
        /// <returns>
        /// Returns course model that contains:
        /// id, title, level, durationInMonths, description, startDate, endDate, languageName, teacherName, students, capacity, isInCourse(bool that checks if user is already signed for this course)
        /// If not found returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Get specific information about selected course for admins.
        /// </summary>
        /// <param name="id">Selected course id</param>
        /// <returns>
        /// Returns course model that contains:
        /// id, title, level, durationInMonths, description, startDate, endDate, languageName, teacherName
        /// If not found returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Get information about students that are in specific course.
        /// </summary>
        /// <param name="id">Selected course id</param>
        /// <returns>
        /// Returns json object with:
        /// 1.Course id
        /// 2.Course Title
        /// 3.Collection of objects with infromation about students:
        ///  3.1.Student id
        ///  3.2.Full name
        ///  3.3.Email
        /// If course is not found returns json object with property message.
        /// </returns>
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
                return NotFound(new { message = "Course is not found!" });
            }
        }

        /// <summary>
        /// Removes teacher from course.
        /// </summary>
        /// <param name="id">Selected course id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Updates course details.
        /// </summary>
        /// <param name="model">Object that contains:
        /// 1. Course id
        /// 2. Title
        /// 3. Level
        /// 4. Duration in months
        /// 5. Description
        /// 6. Start date
        /// </param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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

        /// <summary>
        /// Removes student from course
        /// </summary>
        /// <param name="courseId">Selected course id</param>
        /// <param name="userId">Selected user id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
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
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = dbEx.Message });
            }

            return Ok(new { message = "User is removed from the course." });
        }
    }
}
