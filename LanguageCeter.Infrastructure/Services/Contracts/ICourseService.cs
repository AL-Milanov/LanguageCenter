using LanguageCenter.Infrastructure.Models;

namespace LanguageCenter.Infrastructure.Services.Contracts
{
    public interface ICourseService
    {
        Task<ICollection<AllCourseVM>> GetAllAsync();

        Task AddAsync(AddCourseVM model);
    }
}
