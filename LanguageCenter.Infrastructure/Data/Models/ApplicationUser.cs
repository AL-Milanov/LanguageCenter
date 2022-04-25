using LanguageCenter.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(Constraints.ApplicationUser.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(Constraints.ApplicationUser.LastNameMaxLength)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }

        public ApplicationUser()
        {
            Courses = new List<Course>();
            Certificates = new List<Certificate>();
        }
    }
}
