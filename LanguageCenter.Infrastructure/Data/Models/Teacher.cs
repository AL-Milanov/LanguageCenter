using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Infrastructure.Data.Models
{
    public class Teacher
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string UserId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(UserId))]
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