using LanguageCenter.Core.Data.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(Constraints.ApplicationUser.firstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(Constraints.ApplicationUser.lastNameMaxLength)]
        public string LastName { get; set; }
    }
}
