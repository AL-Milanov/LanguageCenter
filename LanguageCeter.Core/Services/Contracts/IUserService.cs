using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Infrastructure.Data.Models;
using System.Linq.Expressions;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserVM>> GetAll(Expression<Func<ApplicationUser, bool>> search = null);

        Task<UserDetailsVM> GetUserDetails(string id);

        Task<UserCoursesVM> GetAllUserCourses(string id);

        Task JoinCourse(string userId, string courseId);
    }
}
