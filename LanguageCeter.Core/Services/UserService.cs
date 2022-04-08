using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationRepository _repo;
        public UserService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserVM>> GetAll()
        {
            var users = await _repo
                .GetAll<ApplicationUser>()
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FirstName + "" + u.LastName,
                })
                .ToListAsync();

            return users;

        }

        public async Task<UserDetailsVM> GetUserDetails(string id)
        {
            var user = await _repo
                .GetAll<ApplicationUser>()
                .Where(u => u.Id == id)
                .Select(u => new UserDetailsVM
                {
                    Email = u.Email,
                    FullName = u.FirstName,
                    Id = u.Id,
                })
                .FirstOrDefaultAsync();


            return user;
        }

        public async Task<bool> MakeTeacher(string id)
        {
            var teacher = new Teacher()
            {
                TeacherId = id
            };

            var result = false;

            try
            {
                await _repo.AddAsync(teacher);
                await _repo.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
                throw new Exception("Something happend try again!");
            }

            return result;
        }
    }
}
