using LanguageCenter.Core.Models.LanguageModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ILanguageService
    {
        Task<List<SelectListItem>> GetAllAsSelectListAsync();

        Task<List<LanguageVM>> GetAllAsync();

        Task AddAsync(CreateLanguageVM model);

        Task<bool> DeleteAsync(string id);
    }
}
