namespace LanguageCenter.Core.Models.CourseModels
{
    public class GetCourseVM
    {
        public string Title { get; set; }

        public string Level { get; set; }

        public short DurationInMonths { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string LanguageName { get; set; }

        public string TeacherName { get; set; }
    }
}
