using System.ComponentModel;

namespace LanguageCenter.Core.Models.TeacherModels
{
    public class GetAllTeachersVM
    {
        public string Id { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public List<string> Languages { get; set; }

        public List<string> CoursesTitles { get; set; }
    }
}
