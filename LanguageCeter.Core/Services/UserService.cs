using LanguageCenter.Core.Common;
using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LanguageCenter.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationRepository _repo;
        public UserService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserVM>> GetAll(Expression<Func<ApplicationUser, bool>> search = null)
        {
            var users = await _repo
                .GetAll<ApplicationUser>()
                .Where(search)
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FirstName + " " + u.LastName,
                })
                .ToListAsync();

            return users;

        }

        public async Task<UserCoursesVM> GetAllUserCourses(string id)
        {
            var user = await _repo.GetAll<ApplicationUser>()
                .Where(u => u.Id == id)
                .Select(u => new UserCoursesVM
                {
                    Id = u.Id,
                    ActiveCourses = u.Courses
                        .Where(c => c.EndDate > DateTime.UtcNow)
                        .Select(c => new CourseName
                        {
                            Id=c.Id,
                            Name = c.Title
                        })
                        .ToList(),
                    PastCourses = u.Courses
                        .Where(c => c.EndDate < DateTime.UtcNow)
                        .Select(c => new CourseName
                        {
                            Id = c.Id,
                            Name = c.Title
                        })
                        .ToList(),

                })
                .FirstOrDefaultAsync();

            Guard.AgainstNull(user, nameof(user));

            return user;
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

            Guard.AgainstNull(user, nameof(user));

            return user;
        }

        public async Task JoinCourse(string userId, string courseId)
        {
            var course = await _repo
                .GetAll<Course>()
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            var user = await _repo
                .GetAll<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.Id == userId);

            Guard.AgainstNull(course, nameof(course));
            Guard.AgainstNull(user, nameof(user));

            if (course.Students.Count > course.Capacity)
            {
                throw new InvalidOperationException("Няма повече свободни места в курса.");
            }

            try
            {
                course.Students.Add(user);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
