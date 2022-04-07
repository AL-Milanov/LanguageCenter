using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task<IActionResult> AllLanguages()
        {
            var languages = await _languageService.GetAllAsync();

            return View(languages);
        }

        public IActionResult CreateLanguage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLanguage(CreateLanguageVM model)
        {
            await _languageService.AddAsync(model);

            return RedirectToAction(nameof(AllLanguages));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(string id)
        {
            var result = await _languageService.DeleteAsync(id);

            if (result)
            {
                return RedirectToAction(nameof(AllLanguages));
            }

            return RedirectToAction(nameof(AllLanguages));
        }
    }
}
