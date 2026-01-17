using System.ComponentModel.DataAnnotations;

namespace Bricouli.Areas.Admin.Models
{
    public class AdminUserListItem
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }

    public class AdminUserEditViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Role { get; set; }

        public List<string> AvailableRoles { get; set; } = new();
    }
}
