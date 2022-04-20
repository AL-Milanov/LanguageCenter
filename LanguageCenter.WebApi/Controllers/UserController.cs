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

        [HttpGet]
        [Route("get-user-details")]
        public async Task<IActionResult> GetUserDetails([FromQuery] string id = null)
        {
            try
            {
                var user = await _userService.GetUserDetails(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAll([FromQuery]string id)
        {
            Expression<Func<ApplicationUser, bool>> search = u => u.Id != id;

            var users = await _userService.GetAll(search);

            return Ok(users);
        }
    }
}
