using LanguageCenter.Core.Common;
using LanguageCenter.Core.Common.ExceptionMessages;
using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Models.TeacherModels;
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
                Name = model.Name,
                NormalizedName = model.Name.ToUpper().Trim(),
            };

            try
            {
                await _repo.AddAsync(language);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException(ExceptionMessage.DbException);
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = false;

            var language = await _repo.GetByIdAsync<Language>(id);

            Guard.AgainstNull(language, nameof(language));

            try
            {
                result = await _repo.Delete<Language>(language.Id);

                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException(ExceptionMessage.DbException);
            }

            return result;
        }

        public async Task<bool> Exists(string languageName)
        {
            var normalizedName = languageName.ToUpper().Trim();

            var language = await _repo.GetAll<Language>()
                .FirstOrDefaultAsync(l => l.NormalizedName == normalizedName);

            Guard.AgainstNull(language, nameof(language));

            return true;
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
                selectListItems.Add(new SelectListItem() 
                { 
                    Text = language.Name,
                    Value = language.Name 
                });
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

        public async Task<ICollection<SelectListItem>> GetAllTeachersByLanguage(string searchedLanguage)
        {
            var normalizedLanguage = searchedLanguage.ToUpper();

            var teachers = await _repo
                .GetAll<Teacher>()
                .Include(t => t.User)
                .Include(t => t.Languages)
                .Select(l => new GetTeachersByLanguagesVM
                {
                    Id = l.Id,
                    FullName = l.User.FirstName + " " + l.User.LastName,
                    Languages = l.Languages.Select(l => l.NormalizedName)
                })
                .Where(t => t.Languages.Contains(normalizedLanguage))
                .ToListAsync();

            var teachersListItems = teachers.Select(t => new SelectListItem
            {
                Text = t.FullName,
                Value = t.Id
            })
                .ToList();

            return teachersListItems;
        }
    }
}
