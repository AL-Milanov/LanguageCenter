using LanguageCenter.Core.Common;
using LanguageCenter.Core.Models.TeacherModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private const int _pageResults = 8;

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
                .Include(t => t.Languages)
                .FirstOrDefaultAsync(t => t.Id == id);

            Guard.AgainstNull(teacher, nameof(teacher));

            try
            {

                foreach (var language in languages)
                {
                    teacher.Languages.Add(language);
                }

                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Cannot add languages to teacher try again!");
            }
        }

        public async Task RemoveLanguagesFromTeacher(string id)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .Include(t => t.Languages)
                .FirstOrDefaultAsync(t => t.Id == id);

            Guard.AgainstNull(teacher, nameof(teacher));

            try
            {
                teacher.Languages.Clear();

                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Cannot remove languages from teacher, try again.");
            }

        }

        public async Task<TeacherResponse> GetAllTeachers(int page)
        {
            var pageCount = Math.Ceiling(await _repo.GetAll<Teacher>().CountAsync() / (double)_pageResults);

            var teachers = await _repo
                .GetAll<Teacher>()
                .Include(t => t.Languages)
                .Include(t => t.Courses)
                .Include(t => t.User)
                .Skip((page - 1) * _pageResults)
                .Take(_pageResults)
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

            var response = new TeacherResponse
            {
                Teachers = teachers,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
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

            Guard.AgainstNull(teacher, nameof(teacher));

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

            Guard.AgainstNull(teacher, nameof(teacher));

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

        public async Task<bool> MakeInactive(string id)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == id);

            Guard.AgainstNull(teacher, nameof(teacher));

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

        public async Task<ICollection<MeetTeachersVM>> GetAllActiveTeachers()
        {
            var activeTeachers = await _repo
                .GetAll<Teacher>()
                .Include(t => t.Languages)
                .Include(t => t.Courses)
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .Select(t => new MeetTeachersVM
                {
                    FullName = t.User.FirstName + " " + t.User.LastName,
                    Description = t.Description
                })
                .ToListAsync();


            return activeTeachers;
        }

        public async Task EditDescription(string teacherId, string userId, string description)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .Where(t => t.IsActive)
                .FirstOrDefaultAsync(t => t.Id == teacherId);

            Guard.AgainstNull(teacher, nameof(teacher));

            if (teacher.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this description.");
            }

            try
            {
                teacher.Description = description;
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Something happend try again!");
            }
        }

        public async Task EditDescription(string teacherId, string description)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .Where(t => t.IsActive)
                .FirstOrDefaultAsync(t => t.Id == teacherId);

            Guard.AgainstNull(teacher, nameof(teacher));

            try
            {
                teacher.Description = description;
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Something happend try again!");
            }
        }

        public async Task<TeacherDescriptionVM> GetTeacherDescription(string teacherId)
        {
            var teacher = await _repo.GetAll<Teacher>()
                .Where(t => t.Id == teacherId)
                .Select(t => new TeacherDescriptionVM
                {
                    Description = t.Description,
                    TeacherId = t.Id,
                    UserId = t.UserId
                })
                .FirstOrDefaultAsync();

            Guard.AgainstNull(teacher, nameof(Teacher));

            return teacher;
        }
    }
}
