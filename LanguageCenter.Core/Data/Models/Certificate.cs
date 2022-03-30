using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Data.Models
{
    public class Certificate
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; }

        [Required]
        [StringLength(Constraints.Course.TitleMaxLength)]
        public string CourseTitle { get; set; }

        [Required]
        [StringLength(Constraints.ApplicationUser.FirstNameMaxLength 
            + Constraints.ApplicationUser.LastNameMaxLength)]
        public string StudentFullName { get; set; }

        public DateTime IssueDate { get; set; }

        public double Grage { get; set; }
    }
}
