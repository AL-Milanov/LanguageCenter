using LanguageCenter.Core.Models.LanguageModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ILanguageService
    {
        Task<List<SelectListItem>> GetAllAsync();

        Task AddAsync(LanguageVM model);

        Task<bool> DeleteAsync(string id);
    }
}
