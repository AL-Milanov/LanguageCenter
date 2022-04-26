using System.ComponentModel;

namespace LanguageCenter.Core.Models.CourseModels
{
    public class JoinCourseVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Level { get; set; }

        [DisplayName("Duration In Months")]
        public short DurationInMonths { get; set; }

        public string Description { get; set; }

        [DisplayName("Start Date")]
        public string StartDate { get; set; }

        [DisplayName("End Date")]
        public string EndDate { get; set; }

        [DisplayName("Language Name")]
        public string LanguageName { get; set; }

        [DisplayName("Teacher Name")]
        public string TeacherName { get; set; }

        public int Students { get; set; }

        public int Capacity { get; set; }

        public bool IsInCourse { get; set; }
    }
}
