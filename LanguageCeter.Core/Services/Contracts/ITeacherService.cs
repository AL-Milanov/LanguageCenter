using LanguageCenter.Core.Models.TeacherModels;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ITeacherService
    {
        Task<bool> MakeTeacher(string id);

        Task<IEnumerable<GetAllTeachersVM>> GetAllTeachers();

        Task<GetTeacherVM> GetTeacher(string id);
    }
}
