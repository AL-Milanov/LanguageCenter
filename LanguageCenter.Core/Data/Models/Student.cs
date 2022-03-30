using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Core.Data.Models
{
    public class Student
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
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