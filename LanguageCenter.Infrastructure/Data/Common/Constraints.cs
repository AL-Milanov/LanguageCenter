namespace LanguageCenter.Infrastructure.Data.Common
{
    public static class Constraints
    {
        public const int GuidIdLenght = 450;

        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 55;
                             
            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 70;
        }

        public static class Course
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 40;

            public const int LevelLength = 2;

            public const int DescriptionLength = 500;

            public const short RangeMinValue = 2;
            public const short RangeMaxValue = 6;

            public const short Capacity = 12;
        }

        public static class Language
        {
            public const int LanguageNameLength = 25;
        }

        public static class Teacher
        {
            public const int DescriptionLength = 400;
        }

        public static class Role
        {
            public const string Admin = "Admin";
        }
    }
}
