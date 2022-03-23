namespace LanguageCenter.Core.Data.Common
{
    public static class Constraints
    {
        public const int GuidIdLenght = 36;

        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 55;
                             
            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 70;
        }
    }
}
