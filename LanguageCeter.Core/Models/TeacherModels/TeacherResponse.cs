namespace LanguageCenter.Core.Models.TeacherModels
{
    public class TeacherResponse
    {

        public List<GetAllTeachersVM> Teachers{ get; set; }

        public int CurrentPage { get; set; }

        public int Pages { get; set; }


        public TeacherResponse()
        {
            Teachers = new List<GetAllTeachersVM>();
        }
    }
}
