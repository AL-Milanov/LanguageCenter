using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult AllUsers()
        {
            return View();
        }
    }
}
