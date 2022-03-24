using LanguageCenter.Core.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Data.Models
{
    public class Language
    {
        [Key]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(Constraints.Language.LanguageNameLength)]
        public string Name { get; set; }
    }
}