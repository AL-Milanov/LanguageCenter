using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Infrastructure.Data.Models
{
    public class Language
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(Constraints.Language.LanguageNameLength)]
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }

        public Language()
        {
            Teachers = new HashSet<Teacher>();
        }
    }
}