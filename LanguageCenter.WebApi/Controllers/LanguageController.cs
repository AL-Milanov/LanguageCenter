﻿using LanguageCenter.Core.Models.LanguageModels;
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

        [HttpPost]
        [Route("add-language")]
        public async Task<IActionResult> AddLanguageAsync(CreateLanguageVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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