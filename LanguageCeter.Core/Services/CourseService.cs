using LanguageCenter.Core.Common;
using LanguageCenter.Core.Common.ExceptionMessages;
using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApplicationRepository _repo;

        private const int _pageResults = 6;

        public CourseService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(AddCourseVM model)
        {
            var language = await _repo.GetAll<Language>()
                .FirstOrDefaultAsync(l => l.NormalizedName == model.LanguageName.ToUpper());

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
                Language = language
            };

            course.EndDate = course.StartDate.AddMonths(course.DurationInMonths);


            try
            {
                await _repo.AddAsync(course);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException(ExceptionMessage.DbException);
            }

        }

        public async Task<Course> AddTeacherToCourse(string courseId, string teacherId)
        {
            var course = await _repo.GetAll<Course>()
                .FirstOrDefaultAsync(c => c.Id == courseId);

            var teacher = await _repo.GetAll<Teacher>()
                .FirstOrDefaultAsync(t => t.Id == teacherId);

            Guard.AgainstNull(course, nameof(course));
            Guard.AgainstNull(teacher, nameof(teacher));

            course.Teacher = teacher;

            try
            {
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException(ExceptionMessage.DbException);
            }

            return course;
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
                throw new DbUpdateException(ExceptionMessage.DbException);
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

        public async Task<CourseResponse> GetAllActiveAsync(int page)
        {

            var pageCount = Math.Ceiling(await _repo.GetAll<Course>().CountAsync() / (double)_pageResults);

            var courses = await _repo
                .GetAll<Course>()
                .Include(c => c.Language)
                .Where(c => c.EndDate > DateTime.UtcNow)
                .OrderByDescending(c => c.StartDate)
                .Skip((page - 1) * _pageResults)
                .Take(_pageResults)
                .Select(c => new AllCourseVM
                {
                    Id = c.Id,
                    LanguageName = c.Language.Name + ".png",
                    Level = c.Level,
                    Title = c.Title,
                    StartDate = c.StartDate.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            var response = new CourseResponse
            {
                Courses = courses,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }

        public async Task<ICollection<AllCourseVM>> GetCoursesByLanguageAsync(string language)
        {
            var courses = await _repo
                .GetAll<Course>()
                .Include(c => c.Language)
                .Where(c => c.EndDate > DateTime.UtcNow)
                .Where(c => c.Language.NormalizedName == language.ToUpper())
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

        public async Task<JoinCourseVM> GetByIdAsync(string courseId, string userId)
        {
            var course = await _repo.GetAll<Course>()
                .Include(c => c.Language)
                .Include(c => c.Students)
                .Include(c => c.Teacher)
                .ThenInclude(c => c.User)
                .Where(c => c.Id == courseId)
                .FirstOrDefaultAsync();

            var user = await _repo.GetAll<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.Id == userId);

            Guard.AgainstNull(course, nameof(course));
            Guard.AgainstNull(user, nameof(user));

            var teacherFullName = course.Teacher?.User?.FirstName + " " + course.Teacher?.User?.LastName ?? null;

            var courseVM = new JoinCourseVM()
            {
                Id = course.Id,
                Description = course.Description,
                DurationInMonths = course.DurationInMonths,
                LanguageName = course.Language.Name,
                Level = course.Level,
                Title = course.Title,
                StartDate = course.StartDate.ToString("dd/MM/yyyy"),
                EndDate = course.EndDate.ToString("dd/MM/yyyy"),
                TeacherName = teacherFullName,
                Capacity = course.Capacity,
                Students = course.Students.Count(),
                IsInCourse = course.Students.Contains(user)
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
                throw new DbUpdateException(ExceptionMessage.DbException);
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
                throw new DbUpdateException(ExceptionMessage.DbException);
            }
        }

        public async Task<GetCourseVM> GetDetailsAsync(string courseId)
        {
            var course = await _repo.GetAll<Course>()
                 .Include(c => c.Language)
                 .Include(c => c.Teacher)
                 .ThenInclude(c => c.User)
                 .FirstOrDefaultAsync(c => c.Id == courseId);

            Guard.AgainstNull(course, nameof(course));

            var teacherFullName = course.Teacher?.User?.FirstName + " " + course.Teacher?.User?.LastName ?? null;

            var courseVM = new GetCourseVM()
            {
                Id = course.Id,
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

        public async Task<CourseStudentsVM> GetStudentsFromCourseAsync(string id)
        {
            var course = await _repo.GetAll<Course>()
                .Select(c => new CourseStudentsVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    Students = c.Students
                        .Select(s => new UserVM
                        {
                            Id = s.Id,
                            Email = s.Email,
                            FullName = s.FirstName + " " + s.LastName
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            Guard.AgainstNull(course, nameof(course));

            return course;
        }

        public async Task<Course> RemoveStudentFromCourseAsync(string courseId, string userId)
        {
            var course = await _repo.GetAll<Course>()
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            Guard.AgainstNull(course, nameof(course));

            if (!course.Students.Any(s => s.Id == userId))
            {
                throw new ArgumentException("This student is not signed for this course.");
            }

            var user = await _repo.GetAll<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.Id == userId);

            Guard.AgainstNull(user, nameof(user));

            try
            {
                var userToRemove = course.Students.FirstOrDefault(s => s.Id == userId);

                course.Students.Remove(userToRemove);
                await _repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new DbUpdateException(ExceptionMessage.DbException);
            }

            return course;
        }
    }
}
