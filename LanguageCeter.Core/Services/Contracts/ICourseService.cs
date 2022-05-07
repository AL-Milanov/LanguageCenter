using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Infrastructure.Data.Models;

namespace LanguageCenter.Infrastructure.Services.Contracts
{
    public interface ICourseService
    {
        Task<ICollection<AllCourseVM>> GetAllAsync();

        Task<CourseResponse> GetAllActiveAsync(int page);

        Task<ICollection<AllCourseVM>> GetCoursesByLanguageAsync(string language);

        Task AddAsync(AddCourseVM model);

        Task<JoinCourseVM> GetByIdAsync(string courseId, string userId);

        Task<GetCourseVM> GetDetailsAsync(string courseId);

        Task<bool> DeleteAsync(string id);

        Task<Course> AddTeacherToCourse(string courseId, string teacherId);

        Task RemoveTeacherFromCourse(string courseId);

        Task UpdateCourse(EditCourseInfoVM model);

        Task<CourseStudentsVM> GetStudentsFromCourseAsync(string id);

        Task<Course> RemoveStudentFromCourseAsync(string courseId, string userId);
    }
}
