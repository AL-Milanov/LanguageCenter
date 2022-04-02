using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCenter.Infrastructure.Data.Models
{
    public class Course
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(Constraints.Course.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(Constraints.Course.LevelLength)]
        public string Level { get; set; }

        public short DurationInMonths { get; set; }

        [StringLength(Constraints.Course.DescriptionLength)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(Constraints.GuidIdLenght)]
        public string TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; }

        [StringLength(Constraints.GuidIdLenght)]
        public string LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        public ICollection<Student> Students { get; set; }

        public Course()
        {
            EndDate = StartDate.AddMonths(DurationInMonths);

            Students = new HashSet<Student>();
        }

    }
}
