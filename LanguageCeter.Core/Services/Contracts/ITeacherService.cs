using LanguageCenter.Core.Models.TeacherModels;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ITeacherService
    {
        Task<bool> MakeTeacher(string id);

        Task<ICollection<GetAllTeachersVM>> GetAllTeachers();

        Task<GetTeacherVM> GetTeacher(string id);

        Task<ICollection<string>> GetTeachersId();

        Task RemoveLanguagesFromTeacher(string id);

        Task AddLanguagesToTeacher(string id, ICollection<string> languagesNames);

        Task<bool> MakeUnactive(string id);

        Task<bool> MakeActive(string id);
    }
}
