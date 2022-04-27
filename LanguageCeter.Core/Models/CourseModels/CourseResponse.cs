namespace LanguageCenter.Core.Models.CourseModels
{
    public class CourseResponse
    {
        public List<AllCourseVM> Courses { get; set; }

        public int CurrentPage { get; set; }

        public int Pages { get; set; }

        public CourseResponse()
        {
            Courses = new List<AllCourseVM>();
        }
    }
}
