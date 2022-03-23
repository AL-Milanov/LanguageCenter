namespace LanguageCenter.Core.Data.Common
{
    public static class Constraints
    {

        public static class ApplicationUser
        {
            public const int firstNameMinLength = 2;
            public const int firstNameMaxLength = 55;

            public const int lastNameMinLength = 2;
            public const int lastNameMaxLength = 70;
        }
    }
}
