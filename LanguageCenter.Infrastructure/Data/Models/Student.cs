using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Infrastructure.Data.Models
{
    public class Student
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; }

        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }

        public Student()
        {
            Courses = new HashSet<Course>();
            Certificates = new HashSet<Certificate>();
        }
    }
}