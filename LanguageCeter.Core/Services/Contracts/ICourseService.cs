using LanguageCenter.Core.Models.CourseModels;

namespace LanguageCenter.Infrastructure.Services.Contracts
{
    public interface ICourseService
    {
        Task<ICollection<AllCourseVM>> GetAllAsync();

        Task<ICollection<AllCourseVM>> GetAllActiveAsync();

        Task<ICollection<AllCourseVM>> GetCoursesByLanguageAsync(string language);

        Task AddAsync(AddCourseVM model);

        Task<JoinCourseVM> GetByIdAsync(string courseId, string userId);

        Task<bool> DeleteAsync(string id);

        Task<bool> AddTeacherToCourse(string courseId, string teacherId);

        Task RemoveTeacherFromCourse(string courseId);

        Task UpdateCourse(EditCourseInfoVM model);
    }
}
