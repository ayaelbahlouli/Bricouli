using System.ComponentModel.DataAnnotations;

namespace Bricouli.Models
{
    public class DevisRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string Email { get; set; } = string.Empty;

     [Required(ErrorMessage = "Le téléphone est requis")]
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

      [Required(ErrorMessage = "La catégorie est requise")]
      [StringLength(50)]
      public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le lieu est requis")]
     [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public int Budget { get; set; }

    [Required(ErrorMessage = "La description est requise")]
        [StringLength(1000)]
 public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'urgence est requise")]
        [StringLength(50)]
        public string Urgency { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "pending"; // pending, accepted, declined
    }
}
