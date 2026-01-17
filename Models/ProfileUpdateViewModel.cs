using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bricouli.Models
{
    public class ProfileUpdateViewModel
    {
        [StringLength(120)]
        public string? FullName { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        public IFormFile? AvatarFile { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; }

        [StringLength(20)]
        public string? AccountType { get; set; }
    }
}
