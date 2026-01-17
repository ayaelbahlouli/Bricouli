using System.ComponentModel.DataAnnotations;

namespace Bricouli.Models
{
    public class ContactForm
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;
    }
}
