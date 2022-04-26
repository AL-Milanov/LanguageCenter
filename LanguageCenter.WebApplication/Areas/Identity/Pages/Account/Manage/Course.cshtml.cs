using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.WebApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LanguageCenter.WebApplication.Areas.Identity.Pages.Account.Manage
{
    public class CourseModel : PageModel
    {
        private readonly HttpClient _client;

        public List<CourseName> ActiveCourses { get; set; }

        public List<CourseName> PastCourses { get; set; }

        public CourseModel(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _client.GetAsync($"/User/get-user-courses?id={User.GetId()}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<UserCoursesVM>(result);

                LoadAsync(user);
                return Page();
            }

            return NotFound();
        }

        private void LoadAsync(UserCoursesVM? user)
        {
            if (user != null)
            {
                ActiveCourses = user.ActiveCourses;

                PastCourses = user.PastCourses;
            }
        }
    }
}
