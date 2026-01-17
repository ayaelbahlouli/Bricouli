using Bricouli.Data;
using Bricouli.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bricouli.Pages
{
    [Authorize]
    public class BecomeProviderModel : PageModel
    {
        private readonly BricoiliDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public BecomeProviderModel(BricoiliDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public ProviderApplication Application { get; set; } = new();

        public ProviderApplication? ExistingApplication { get; private set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return;
            }

            ExistingApplication = await _dbContext.ProviderApplications
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var existing = await _dbContext.ProviderApplications
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (existing != null)
            {
                TempData["Success"] = "Votre demande est deja en cours de traitement.";
                return RedirectToPage("/BecomeProvider");
            }

            Application.UserId = user.Id;
            Application.Name = User.FindFirst("FullName")?.Value ?? user.UserName ?? user.Email ?? "Utilisateur";
            Application.Email = user.Email ?? string.Empty;
            Application.CreatedAt = DateTime.UtcNow;
            Application.Status = "new";

            ModelState.Clear();
            if (!TryValidateModel(Application, nameof(Application)))
            {
                return Page();
            }

            _dbContext.ProviderApplications.Add(Application);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = "Votre demande a ete envoyee.";

            return RedirectToPage("/BecomeProvider");
        }
    }
}
