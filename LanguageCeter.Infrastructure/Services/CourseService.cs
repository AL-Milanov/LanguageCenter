using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Infrastructure.Services
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

            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == model.TeacherId);

            if (language == null)
            {
                throw new ArgumentException("Language does not exist!");
            }

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
                .Select(c => new AllCourseVM
                {
                    Id = c.Id,
                    LanguageName = c.Language.Name,
                    Level = c.Level,
                    Title = c.Title,
                    StartDate = c.StartDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return courses;
        }

        public async Task<GetCourseVM> GetByIdAsync(string id)
        {
            var course = await _repo.GetByIdAsync<Course>(id);

            var courseVM = new GetCourseVM()
            {
                Description = course.Description,
                DurationInMonths = course.DurationInMonths,
                LanguageName = course.Language.Name,
                Level = course.Level,
                Title = course.Title,
                StartDate = course.StartDate.ToString("dd/MM/yyyy"),
                EndDate = course.EndDate.ToString("dd/MM/yyyy")
            };

            return courseVM;
        }
    }
}
