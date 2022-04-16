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

        public async Task AddLanguagesToTeacher(string id, ICollection<string> languagesNames)
        {
            var languages = await _repo
                .GetAll<Language>()
                .Where(language => languagesNames.Contains(language.Name))
                .ToListAsync();

            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                throw new InvalidOperationException("Teacher not found");
            }

            try
            {
                await RemoveLanguagesFromTeacher(teacher);

                foreach (var language in languages)
                {
                    teacher.Languages.Add(language);
                    language.Teachers.Add(teacher);
                }

                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Cannot add languages to teacher try again!");
            }
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
                    IsActive = t.IsActive,
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
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                throw new InvalidOperationException("Teacher not found!");
            }

            return teacher;
        }

        public async Task<ICollection<string>> GetTeachersId()
        {
            var teachersIds = await _repo.GetAll<Teacher>()
                .Select(t => t.UserId)
                .ToListAsync();

            return teachersIds;
        }

        public async Task<bool> MakeActive(string id)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                throw new InvalidOperationException("Teacher not found");
            }

            try
            {
                teacher.IsActive = true;
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Teacher status is not updated try again!");
            }

            return teacher.IsActive;
        }

        public async Task<bool> MakeTeacher(string id)
        {
            var teacher = new Teacher()
            {
                UserId = id
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
                throw new DbUpdateException("Something happend try again!");
            }

            return result;
        }

        public async Task<bool> MakeUnactive(string id)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                throw new InvalidOperationException("Teacher not found.");
            }

            try
            {
                teacher.IsActive = false;
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Problem occured try again!");
            }

            return teacher.IsActive;
        }

        private async Task RemoveLanguagesFromTeacher(Teacher teacher)
        {

            try
            {
                teacher.Languages.Clear();
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Problem occured try again!");
            }

        }
    }
}
