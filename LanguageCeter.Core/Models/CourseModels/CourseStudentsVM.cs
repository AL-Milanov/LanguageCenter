using LanguageCenter.Core.Models.UserModels;

namespace LanguageCenter.Core.Models.CourseModels
{
    public class CourseStudentsVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<UserVM> Students { get; set; }

    }
}
