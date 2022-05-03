using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace LanguageCenter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get specific user details
        /// </summary>
        /// <param name="id">User id</param>
        /// Returns object with info about user which contains:
        /// 1.Id
        /// 2.FullName
        /// 3.Email
        /// 4.Array of strings containing role names
        /// If user not found returns object (string)message
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-details")]
        public async Task<IActionResult> GetUserDetails([FromQuery] string id)
        {
            try
            {

                var user = await _userService.GetUserDetails(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get details for all users except currently logged
        /// </summary>
        /// <param name="id">Current user id</param>
        /// <param name="page">Page you want to get</param>
        /// <returns>
        /// Returns object with:
        /// 1.Page - int max 
        /// 2.Current page
        /// 3.Collection of user details:
        ///  3.1.Id
        ///  3.2.FullName
        ///  3.3.Email
        /// </returns>
        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAll([FromQuery] string id, int page)
        {
            Expression<Func<ApplicationUser, bool>> search = u => u.Id != id;

            var users = await _userService.GetAll(page, search);

            return Ok(users);
        }

        /// <summary>
        /// Get all courses that the user is signed for
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>
        /// Returns object with:
        /// 1.User id
        /// 2.Collection of object with currently active courses names
        /// 3.Collection of object with past courses names
        /// If user is not found returns json object which contains (string)message property.
        /// </returns>
        [HttpGet]
        [Route("get-user-courses")]
        public async Task<IActionResult> GetUserCoursesAsync([FromQuery] string id)
        {
            try
            {

                var user = await _userService.GetAllUserCourses(id);

                return Ok(user);
            }
            catch (Exception ex)
            {

                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Makes user to join specific course
        /// </summary>
        /// <param name="userId">Logged user id</param>
        /// <param name="courseId">Course id to join</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
        [HttpPost]
        [Route("join-course")]
        public async Task<IActionResult> JoinCourseAsync([FromQuery] string userId, [FromQuery] string courseId)
        {
            try
            {

                await _userService.JoinCourse(userId, courseId);
            }
            catch (ArgumentNullException arEx)
            {

                return NotFound(new { message = arEx.Message });
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { message = "Успешно се записахте за курса." });
        }
    }
}
