using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> AllUsers()
        {

            var users = await _userService
                .GetAll(u => u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(users);
        }
    }
}
