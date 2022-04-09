using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IApplicationRepository _repo;

        public TeacherService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ICollection<GetAllTeachersVM>> GetAllTeachers()
        {
            var teachers = await _repo
                .GetAll<Teacher>()
                .Include(t => t.Languages)
                .Include(t => t.Courses)
                .Include(t => t.User)
                .Select(t => new GetAllTeachersVM
                {
                    Id = t.Id,
                    FullName = t.User.FirstName + " " + t.User.LastName,
                    Email = t.User.Email,
                    Languages = t.Languages
                        .Select(l => l.Name)
                        .ToList(),
                    CoursesTitles = t.Courses
                        .Select(c => c.Title)
                        .ToList(),
                })
                .ToListAsync();

            return teachers;
        }

        public async Task<GetTeacherVM> GetTeacher(string id)
        {
            var teacher = await _repo
                .GetAll<Teacher>()
                .Include(t => t.Languages)
                .Include(t => t.Courses)
                .Include(t => t.User)
                .Select(t => new GetTeacherVM
                {
                    Id = t.Id,
                    FullName = t.User.FirstName + " " + t.User.LastName,
                    Languages = t.Languages
                        .Select(l => l.Name)
                        .ToList(),
                    CoursesTitles = t.Courses
                        .Select(c => c.Title)
                        .ToList(),
                })
                .FirstOrDefaultAsync();

            return teacher;
        }

        public async Task<ICollection<string>> GetTeachersId()
        {
            var teachersIds = await _repo.GetAll<Teacher>()
                .Select(t => t.TeacherId)
                .ToListAsync();

            return teachersIds;
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
