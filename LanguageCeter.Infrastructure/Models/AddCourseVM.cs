using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Infrastructure.Models
{
    public class AddCourseVM
    {
        [Required]
        [StringLength(Constraints.Course.TitleMaxLength,
            MinimumLength = Constraints.Course.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(Constraints.Course.LevelLength,
            MinimumLength = Constraints.Course.LevelLength)]
        public string Level { get; set; }

        [Range(Constraints.Course.RangeMinValue, Constraints.Course.RangeMaxValue)]
        public short DurationInMonths { get; set; }

        [StringLength(Constraints.Course.DescriptionLength)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        [Required]
        [StringLength(Constraints.Language.LanguageNameLength)]
        public string LanguageName { get; set; }

        public string TeacherId { get; set; }
    }
}
