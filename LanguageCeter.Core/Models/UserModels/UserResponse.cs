namespace LanguageCenter.Core.Models.UserModels
{
    public class UserResponse
    {
        public List<UserVM> Users { get; set; }

        public int CurrentPage { get; set; }

        public int Page { get; set; }


        public UserResponse()
        {
            Users = new List<UserVM>();
        }
    }
}
