using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Models.LanguageModels
{
    public class LanguageVM
    {
        [Required]
        [StringLength(Constraints.GuidIdLenght)]
        public string Id { get; set; }

        [Required]
        [StringLength(Constraints.Language.LanguageNameLength)]
        public string Name { get; set; }

    }
}
