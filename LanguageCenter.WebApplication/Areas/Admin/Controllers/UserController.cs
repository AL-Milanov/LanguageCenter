﻿using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.WebApplication.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly HttpClient _client;

        public UserController(
            IUserService userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            HttpClient client)
        {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;

            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> AllUsers()
        {
            //var users = await _userService
            //    .GetAll(u => u.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await _client.GetAsync($"/User/get-all-users?id={User.GetId()}");

            var result = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<List<UserVM>>(result);

            return View(users);
        }

        public async Task<IActionResult> AddRole(string id)
        {
            //var userVM = await _userService.GetUserDetails(id);

            var response = await _client.GetAsync($"/User/get-user-details?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var userVM = JsonConvert.DeserializeObject<UserDetailsVM>(result);

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

            return RedirectToAction(nameof(AllUsers));
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
