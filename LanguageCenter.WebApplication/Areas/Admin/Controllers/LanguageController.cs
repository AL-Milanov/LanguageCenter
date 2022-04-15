using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _languageService;
        private HttpClient _client;

        public LanguageController(
            ILanguageService languageService,
            HttpClient client)
        {
            _languageService = languageService;
            _client = client;
        }

        public async Task<IActionResult> AllLanguages()
        {
            var response = await _client.GetAsync("https://localhost:7188/all-languages");

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

            var response = await _client.PostAsJsonAsync("https://localhost:7188/add-language", model);

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
                .PostAsync($"https://localhost:7188/delete-language/{id}", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(AllLanguages));
            }

            return RedirectToAction(nameof(AllLanguages));
        }
    }
}
