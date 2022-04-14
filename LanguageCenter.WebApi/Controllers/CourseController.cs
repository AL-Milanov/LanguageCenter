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
        [Route("/all-courses")]
        public async Task<IActionResult> GetAllCoursesAsync()
        {
            var courses = await _courseService.GetAllAsync();

            return Ok(courses);
        }

        [HttpPost]
        [Route("/add-course")]
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
            catch (Exception)
            {
                return BadRequest();
            }

            return Created("/addcourse", model);
        }

        [HttpPost]
        [Route("/add-teacher-to-course")]
        public async Task<IActionResult> AddTeacherToCourseAsync(string courseId, string teacherId)
        {
            try
            {
                await _courseService.AddTeacherToCourse(courseId, teacherId);
            }
            catch (ArgumentException)
            {
                return NotFound();
                
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("/delete-course")]
        public async Task<IActionResult> DeleteCourseAsync(string id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("/get-course")]
        public async Task<IActionResult> GetCourseAsync(string id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpPost]
        [Route("/remove-teacher-from-course")]
        public async Task<IActionResult> RemoveTeacherFromCourseAsync(string id)
        {
            try
            {
                await _courseService.RemoveTeacherFromCourse(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("/update-course")]
        public async Task<IActionResult> UpdateCourseAsync(EditCourseInfoVM model)
        {
            try
            {
                await _courseService.UpdateCourse(model);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
