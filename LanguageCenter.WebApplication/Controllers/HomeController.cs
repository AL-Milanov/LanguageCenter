using LanguageCenter.Core.Models.Email;
using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Infrastructure.Data.Common;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.WebApplication.Helper;
using LanguageCenter.WebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LanguageCenter.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpClient _client;
        public HomeController(
            ILogger<HomeController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            HttpClient client)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;

            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Certificate()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact(string? message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(Contact msg)
        {
            var response = await _client.PostAsJsonAsync("/Mail/send-email", msg);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Contact),
                    new { message = "Съобщението бе изпратено успешно! :)" });
            }

            return View(nameof(Contact),
                new { message = "Възникна проблем и съобщението не беше изпратено. Опитайте пак. :(" });
        }

        public async Task<IActionResult> AboutUs()
        {
            var response = await _client.GetAsync("/Teacher/active-teachers");
            var result = await response.Content.ReadAsStringAsync();

            var teachers = JsonConvert.DeserializeObject<List<MeetTeachersVM>>(result);

            return View(teachers);
        }

        //[Authorize(Roles = Constraints.Role.Admin)]
        //public async Task<IActionResult> SetRole()
        //{

        //    var user = await _userManager.GetUserAsync(User);

        //    await _userManager.AddToRoleAsync(user, Constraints.Role.Admin);

        //    return View(nameof(Index));
        //}
    }
}