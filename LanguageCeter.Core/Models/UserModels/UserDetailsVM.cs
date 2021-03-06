namespace LanguageCenter.Core.Models.UserModels
{
    public class UserDetailsVM
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
