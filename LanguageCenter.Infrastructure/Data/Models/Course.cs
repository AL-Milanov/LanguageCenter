using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Core.Data.Models
{
    public class Course
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(25)]
        public string Title { get; set; }

        [Required]
        [StringLength(2)]
        public string Level { get; set; }

        public short DurationInMonths { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; }

        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        public ICollection<Student> Students { get; set; }

        public Course()
        {
            EndDate = StartDate.AddMonths(DurationInMonths);
        }

    }
}
