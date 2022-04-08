using LanguageCenter.Core.Models.UserModels;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserVM>> GetAll();

        Task<UserDetailsVM> GetUserDetails(string id);

        Task<bool> MakeTeacher(string id);

    }
}
