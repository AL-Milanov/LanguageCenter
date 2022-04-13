using System.ComponentModel;

namespace LanguageCenter.Core.Models.CourseModels
{
    public class EditCourseInfoVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Level { get; set; }

        [DisplayName("Duration In Months")]
        public short DurationInMonths { get; set; }

        public string Description { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
    }
}
