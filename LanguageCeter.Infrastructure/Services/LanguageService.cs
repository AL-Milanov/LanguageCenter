using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IApplicationRepository _repo;
        public LanguageService(IApplicationRepository repo)
        {
            _repo = repo;
        }


        public async Task AddAsync(CreateLanguageVM model)
        {
            var language = new Language()
            {
                Name = model.Name
            };

            try
            {
                await _repo.AddAsync(language);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw  new DbUpdateException("Language not saved. Try again later.");
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = false;

            try
            {
                result = await _repo.Delete<Language>(id);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Language not deleted try again later.");
            }

            return result;
        }

        public async Task<List<SelectListItem>> GetAllAsSelectListAsync()
        {
            var languages = await _repo
                .GetAll<Language>()
                .Select(l => new LanguageVM
                {
                    Name = l.Name
                })
                .ToListAsync();

            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (var language in languages)
            {
                selectListItems.Add(new SelectListItem() { Text = language.Name, Value = language.Name});
            }

            return selectListItems;
        }
        public async Task<List<LanguageVM>> GetAllAsync()
        {
            var languages = await _repo
                .GetAll<Language>()
                .Select(l => new LanguageVM
                {
                    Id = l.Id,
                    Name = l.Name
                })
                .ToListAsync();


            return languages;
        }
    }
}
