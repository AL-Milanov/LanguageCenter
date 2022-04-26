using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Models.TeacherModels
{
    public class TeacherDescriptionVM
    {
        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(Constraints.Teacher.DescriptionLength)]
        public string Description { get; set; }

    }
}
