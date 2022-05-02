using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Creates new language
        /// </summary>
        /// <param name="model">
        /// Object with property: (string) name
        /// </param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
        [HttpPost]
        [Route("add-language")]
        public async Task<IActionResult> AddLanguageAsync(CreateLanguageVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (await _languageService.Exists(model.Name))
            {
                return BadRequest(new { message = "Language already exists." });
            }

            try
            {
                await _languageService.AddAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Created("/add-language", new { message = "Language added successfully." });
        }

        /// <summary>
        /// Removes specific language.
        /// </summary>
        /// <param name="id">Selected language id</param>
        /// <returns>
        /// Returns json object which contains (string)message property.
        /// </returns>
        [HttpPost]
        [Route("delete-language")]
        public async Task<IActionResult> DeleteLanguageAsync([FromQuery] string id)
        {
            bool result;

            try
            {
                result = await _languageService.DeleteAsync(id);
            }
            catch (ArgumentException arEx)
            {
                return BadRequest(new { message = arEx.Message });

            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = dbEx.Message });
            }

            return Ok(new { message = "Language deleted successfully." });
        }

        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns>
        /// Returns collection of objects representing language entity.
        /// If any problems occured returns message.
        /// </returns>
        [HttpGet]
        [Route("all-languages")]
        public async Task<IActionResult> AllLanguagesAsync()
        {
            try
            {
                var languages = await _languageService.GetAllAsync();
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all languages as select list item
        /// </summary>
        /// <returns>
        /// Returns collection of object with:
        /// 1.Text
        /// 2.Value
        /// If any problems occured returns message.
        /// </returns>
        [HttpGet]
        [Route("all-languages-as-selected-list")]
        public async Task<IActionResult> GetAllAsSelectListAsync()
        {
            try
            {
                var languages = await _languageService.GetAllAsSelectListAsync();
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        /// <summary>
        /// Get all teachers by specific language
        /// </summary>
        /// <param name="language">Searched language</param>
        /// <returns>
        /// Returns collection of object with:
        /// 1.Text
        /// 2.Value
        /// If any problems occured returns message.
        /// </returns>
        [HttpGet]
        [Route("all-teachers-by-language")]
        public async Task<IActionResult> GetAllTeachersByLanguage([FromQuery] string language)
        {
            try
            {
                var languages = await _languageService.GetAllTeachersByLanguage(language);
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}