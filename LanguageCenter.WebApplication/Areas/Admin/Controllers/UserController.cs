using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> AllUsers()
        {

            var users = await _userService
                .GetAll(u => u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(users);
        }

        public async Task<IActionResult> AddRole(string id)
        {
            var userVM = await _userService.GetUserDetails(id);
            var user = await _userManager.FindByIdAsync(id);

            ViewBag.RoleItems = _roleManager
                .Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = _userManager.IsInRoleAsync(user, r.Name).Result
                })
                .ToList();

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(UserDetailsVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.Roles?.Count > 0)
            {
                await _userManager.AddToRolesAsync(user, model.Roles);
            }

            return RedirectToAction(nameof(AllUsers));
        }
    }
}
