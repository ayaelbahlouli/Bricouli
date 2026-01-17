using System.Collections.Generic;

namespace Bricouli.Models
{
    public class ProfileDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string AccountType { get; set; } = "User";
        public List<string> AvailableAccountTypes { get; set; } = new();
        public bool CanBecomeProfessional { get; set; }
        public List<DevisRequest> Requests { get; set; } = new();
    }
}
