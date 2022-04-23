using System.ComponentModel;

namespace LanguageCenter.Core.Models.CourseModels
{
    public class AllCourseVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Level { get; set; }

        [DisplayName("Language")]
        public string LanguageName { get; set; }

        [DisplayName("Start date")]
        public string StartDate { get; set; }
    }
}
