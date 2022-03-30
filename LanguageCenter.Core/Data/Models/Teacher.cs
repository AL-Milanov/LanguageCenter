using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Core.Data.Models
{
    public class Teacher
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; }

        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Language> Languages { get; set; }

        public Teacher()
        {
            Courses = new HashSet<Course>();
            Languages = new HashSet<Language>();
        }
    }
}