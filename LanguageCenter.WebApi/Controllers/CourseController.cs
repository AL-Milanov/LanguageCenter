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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created("/addcourse", model);
        }

        [HttpPost]
        [Route("/add-teacher-to-course/{courseId}/{teacherId}")]
        public async Task<IActionResult> AddTeacherToCourseAsync(string courseId, string teacherId)
        {
            try
            {
                await _courseService.AddTeacherToCourse(courseId, teacherId);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);

            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.Message);
            }

            return Ok();
        }

        [HttpPost()]
        [Route("/delete-course/{id}")]
        public async Task<IActionResult> DeleteCourseAsync(string id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("/get-course/{id}")]
        public async Task<IActionResult> GetCourseAsync(string id)
        {
            try
            {
                var course = await _courseService.GetByIdAsync(id);

                return Ok(course);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);

            }
        }

        [HttpPost]
        [Route("/remove-teacher-from-course/{id}")]
        public async Task<IActionResult> RemoveTeacherFromCourseAsync(string id)
        {
            try
            {
                await _courseService.RemoveTeacherFromCourse(id);
            }
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.Message);
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
            catch (ArgumentException arEx)
            {
                return NotFound(arEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.Message);
            }

            return Ok();
        }
    }
}
