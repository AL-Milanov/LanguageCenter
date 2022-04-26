using LanguageCenter.Core.Models.TeacherModels;

namespace LanguageCenter.Core.Services.Contracts
{
    public interface ITeacherService
    {
        Task<bool> MakeTeacher(string id);

        Task<ICollection<GetAllTeachersVM>> GetAllTeachers();
        Task<ICollection<MeetTeachersVM>> GetAllActiveTeachers();

        Task<GetTeacherVM> GetTeacher(string id);

        Task<ICollection<string>> GetTeachersId();

        Task AddLanguagesToTeacher(string id, ICollection<string> languagesNames);

        Task<bool> MakeInactive(string id);

        Task<bool> MakeActive(string id);

        Task RemoveLanguagesFromTeacher(string id);

        Task EditDescription(string teacherId, string description);

        Task EditDescription(string teacherId, string userId, string description);

        Task<TeacherDescriptionVM> GetTeacherDescription(string teacherId);

    }
}
