﻿using LanguageCenter.Core.Models.CourseModels;

namespace LanguageCenter.Infrastructure.Services.Contracts
{
    public interface ICourseService
    {
        Task<ICollection<AllCourseVM>> GetAllAsync();

        Task AddAsync(AddCourseVM model);

        Task<GetCourseVM> GetByIdAsync(string id);

        Task<bool> DeleteAsync(string id);

        Task<bool> AddTeacherToCourse(string courseId, string teacherId);

        Task RemoveTeacherFromCourse(string courseId);

        Task UpdateCourse(EditCourseInfoVM model);
    }
}
