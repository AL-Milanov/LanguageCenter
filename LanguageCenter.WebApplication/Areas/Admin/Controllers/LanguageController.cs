using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.WebApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private HttpClient _client;

        public LanguageController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(LanguageCenterApi.uri);
        }

        public async Task<IActionResult> AllLanguages()
        {
            var response = await _client.GetAsync("/Language/all-languages");

            var languages = new List<LanguageVM>();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                languages = JsonConvert.DeserializeObject<List<LanguageVM>>(result);

            }

            return View(languages);
        }

        public IActionResult CreateLanguage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLanguage(CreateLanguageVM model)
        {

            var response = await _client.PostAsJsonAsync("/Language/add-language", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllLanguages));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(string id)
        {
            var response = await _client
                .PostAsync($"/Language/delete-language?id={id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllLanguages));
            }

            return RedirectToAction(nameof(AllLanguages));
        }
    }
}
