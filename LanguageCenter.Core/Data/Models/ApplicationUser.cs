﻿using LanguageCenter.Infrastructure.Data.Common;
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
    }
}
