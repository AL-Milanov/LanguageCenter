using LanguageCenter.Core.Models.CourseModels;

namespace LanguageCenter.Core.Models.UserModels
{
    public class UserCoursesVM
    {

        public string Id { get; set; }

        public List<CourseName> ActiveCourses { get; set; }

        public List<CourseName> PastCourses { get; set; }
    }
}
