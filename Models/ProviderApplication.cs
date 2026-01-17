using System.ComponentModel.DataAnnotations;

namespace Bricouli.Models
{
    public class ProviderApplication
    {
        [Key]
        public int Id { get; set; }

        [StringLength(450)]
        public string? UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Category { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(30)]
        public string Status { get; set; } = "new";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
