using LanguageCenter.Infrastructure.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Models.LanguageModels
{
    public class CreateLanguageVM
    {

        [Required]
        [StringLength(Constraints.Language.LanguageNameLength)]
        public string Name { get; set; }
    }
}
