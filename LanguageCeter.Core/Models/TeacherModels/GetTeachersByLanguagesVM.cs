namespace LanguageCenter.Core.Models.TeacherModels
{
    public class GetTeachersByLanguagesVM
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public IEnumerable<string> Languages { get; set; }
    }
}
