namespace LanguageCenter.Core.Models.TeacherModels
{
    public class GetTeacherVM
    {

        public string Id { get; set; }

        public string FullName { get; set; }

        public List<string> Languages { get; set; }

        public List<string> CoursesTitles { get; set; }
    }
}
