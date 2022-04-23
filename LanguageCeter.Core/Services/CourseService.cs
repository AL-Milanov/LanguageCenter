using LanguageCenter.Core.Common;
using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApplicationRepository _repo;

        public CourseService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(AddCourseVM model)
        {
            var language = await _repo.GetAll<Language>()
                .FirstOrDefaultAsync(l => l.Name == model.LanguageName);

            Guard.AgainstNull(language, nameof(language));

            var course = new Course
            {
                Title = model.Title,
                Level = model.Level,
                DurationInMonths = model.DurationInMonths,
                Description = model.Description,
                LanguageId = language.Id,
                TeacherId = model.TeacherId,
                StartDate = model.StartDate,
            };

            course.EndDate = course.StartDate.AddMonths(course.DurationInMonths);

            course.Language = language;

            try
            {
                await _repo.AddAsync(course);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Something wrong happend! Please try again.");
            }

        }

        public async Task<bool> AddTeacherToCourse(string courseId, string teacherId)
        {
            var course = await _repo.GetAll<Course>()
                .FirstOrDefaultAsync(c => c.Id == courseId);

            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == teacherId);

            var result = true;
            
            Guard.AgainstNull(course, nameof(course));
            Guard.AgainstNull(teacher, nameof(teacher));

            course.Teacher = teacher;

            try
            {
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Cannot update db.");
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = false;

            try
            {
                result = await _repo.Delete<Course>(id);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Problem occurred try again later!");
            }

            return result;
        }

        public async Task<ICollection<AllCourseVM>> GetAllAsync()
        {
            var courses = await _repo
                .GetAll<Course>()
                .Include(c => c.Language)
                .OrderByDescending(c => c.StartDate)
                .Select(c => new AllCourseVM
                {
                    Id = c.Id,
                    LanguageName = c.Language.Name + ".png",
                    Level = c.Level,
                    Title = c.Title,
                    StartDate = c.StartDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return courses;
        }

        public async Task<ICollection<AllCourseVM>> GetAllActiveAsync()
        {
            var courses = await _repo
                .GetAll<Course>()
                .Include(c => c.Language)
                .Where(c => c.EndDate > DateTime.UtcNow)
                .OrderByDescending(c => c.StartDate)
                .Select(c => new AllCourseVM
                {
                    Id = c.Id,
                    LanguageName = c.Language.Name + ".png",
                    Level = c.Level,
                    Title = c.Title,
                    StartDate = c.StartDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return courses;
        }

        public async Task<ICollection<AllCourseVM>> GetCoursesByLanguageAsync(string language)
        {
            var courses = await _repo
                .GetAll<Course>()
                .Include(c => c.Language)
                .Where(c => c.EndDate > DateTime.UtcNow)
                .Where(c => c.Language.Name == language)
                .OrderByDescending(c => c.StartDate)
                .Select(c => new AllCourseVM
                {
                    Id = c.Id,
                    LanguageName = c.Language.Name + ".png",
                    Level = c.Level,
                    Title = c.Title,
                    StartDate = c.StartDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return courses;
        }

        public async Task<GetCourseVM> GetByIdAsync(string id)
        {
            var course = await _repo.GetAll<Course>()
                .Include(c => c.Language)
                .Include(c => c.Teacher)
                .ThenInclude(c => c.User)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            Guard.AgainstNull(course, nameof(course));
           
            var teacherFullName = course.Teacher?.User?.FirstName + " " + course.Teacher?.User?.LastName ?? null;

            var courseVM = new GetCourseVM()
            {
                Id = id,
                Description = course.Description,
                DurationInMonths = course.DurationInMonths,
                LanguageName = course.Language.Name,
                Level = course.Level,
                Title = course.Title,
                StartDate = course.StartDate.ToString("dd/MM/yyyy"),
                EndDate = course.EndDate.ToString("dd/MM/yyyy"),
                TeacherName = teacherFullName
            };

            return courseVM;
        }

        public async Task RemoveTeacherFromCourse(string courseId)
        {
            var course = await _repo.GetAll<Course>()
                .FirstOrDefaultAsync(c => c.Id == courseId);

            Guard.AgainstNull(course, nameof(course));

            course.TeacherId = null;

            try
            {
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Problem occurred try again later!");
            }
        }

        public async Task UpdateCourse(EditCourseInfoVM model)
        {
            var course = await _repo.GetAll<Course>()
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            Guard.AgainstNull(course, nameof(course));

            course.Title = model.Title;
            course.Description = model.Description;
            course.DurationInMonths = model.DurationInMonths;
            course.Level = model.Level;
            course.StartDate = model.StartDate;
            course.EndDate = model.StartDate.AddMonths(model.DurationInMonths);


            try
            {
                _repo.Update(course);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Problem occurred try again later!");
            }
        }
    }
}
