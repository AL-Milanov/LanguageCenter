using LanguageCenter.Core.Models.LanguageModels;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageVM>> GetAllAsync();

        Task AddAsync(LanguageVM model);

        Task<bool> DeleteAsync(string id);
    }
}
