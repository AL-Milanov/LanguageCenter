using System.ComponentModel.DataAnnotations;

namespace LanguageCenter.Core.Models.Email
{
    public class Contact
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
