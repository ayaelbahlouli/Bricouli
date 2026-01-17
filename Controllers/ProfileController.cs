using Bricouli.Data;
using Bricouli.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bricouli.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly BricoiliDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(BricoiliDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [HttpGet("/Profile")]
        public async Task<IActionResult> Index()
        {
            var email = User.Identity?.Name ?? string.Empty;
            var fullName = User.FindFirst("FullName")?.Value ?? string.Empty;
            var avatarUrl = User.FindFirst("AvatarUrl")?.Value ?? string.Empty;

            var requests = await _dbContext.DevisRequests
                .Where(d => d.Email == email)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            var roles = user == null ? new List<string>() : (await _userManager.GetRolesAsync(user)).ToList();
            var accountType = roles.Contains("Professional") ? "Professional" : "User";

            var canBecomeProfessional = false;
            if (user != null)
            {
                canBecomeProfessional = await _dbContext.ProviderApplications
                    .AnyAsync(p => p.UserId == user.Id && p.Status == "approved");
            }

            var model = new ProfileDashboardViewModel
            {
                FullName = fullName,
                Email = email,
                AvatarUrl = avatarUrl,
                AccountType = accountType,
                AvailableAccountTypes = new List<string> { "User", "Professional" },
                CanBecomeProfessional = canBecomeProfessional,
                Requests = requests
            };

            return View(model);
        }

        [HttpPost("/Profile/Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUpdateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var roleChanged = false;
            var claims = await _userManager.GetClaimsAsync(user);
            var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "AvatarUrl");
            var trimmed = model.FullName?.Trim() ?? string.Empty;
            var trimmedAvatar = model.AvatarUrl?.Trim() ?? string.Empty;

            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                var ext = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["ProfileMessage"] = "Format d'image non supporte.";
                    return RedirectToAction(nameof(Index));
                }

                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                Directory.CreateDirectory(uploadsPath);
                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }

                trimmedAvatar = $"/uploads/avatars/{fileName}";
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (fullNameClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, fullNameClaim);
                }
            }
            else if (fullNameClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, fullNameClaim, new Claim("FullName", trimmed));
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim("FullName", trimmed));
            }

            if (string.IsNullOrWhiteSpace(trimmedAvatar))
            {
                if (avatarClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, avatarClaim);
                }
            }
            else if (avatarClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, avatarClaim, new Claim("AvatarUrl", trimmedAvatar));
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim("AvatarUrl", trimmedAvatar));
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && !string.Equals(model.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword) || !await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    TempData["ProfileMessage"] = "Mot de passe actuel invalide pour changer l'email.";
                    return RedirectToAction(nameof(Index));
                }

                user.Email = model.Email.Trim();
                user.UserName = model.Email.Trim();
                var emailResult = await _userManager.UpdateAsync(user);
                if (!emailResult.Succeeded)
                {
                    TempData["ProfileMessage"] = "Impossible de mettre a jour l'email.";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    TempData["ProfileMessage"] = "Mot de passe actuel requis pour changer le mot de passe.";
                    return RedirectToAction(nameof(Index));
                }

                var pwdResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!pwdResult.Succeeded)
                {
                    TempData["ProfileMessage"] = "Impossible de changer le mot de passe.";
                    return RedirectToAction(nameof(Index));
                }
            }

            if (!string.IsNullOrWhiteSpace(model.AccountType))
            {
                var desiredRole = model.AccountType == "Professional" ? "Professional" : "User";
                if (desiredRole == "Professional")
                {
                    var isApproved = await _dbContext.ProviderApplications
                        .AnyAsync(p => p.UserId == user.Id && p.Status == "approved");
                    if (!isApproved)
                    {
                        TempData["ProfileMessage"] = "Votre compte doit etre approuve avant de devenir Professionnel.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Where(r => r != "Admin").ToList();
                if (rolesToRemove.Count > 0)
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        TempData["ProfileMessage"] = "Impossible de modifier le type de compte.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                var addResult = await _userManager.AddToRoleAsync(user, desiredRole);
                if (!addResult.Succeeded)
                {
                    TempData["ProfileMessage"] = "Impossible de modifier le type de compte.";
                    return RedirectToAction(nameof(Index));
                }
                roleChanged = true;
            }

            if (!ModelState.IsValid)
            {
                TempData["ProfileMessage"] = roleChanged
                    ? "Type de compte mis a jour. Verifiez les autres champs du profil."
                    : "Veuillez verifier les informations du profil.";
                return RedirectToAction(nameof(Index));
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["ProfileMessage"] = "Profil mis a jour.";
            return RedirectToAction(nameof(Index));
        }
    }
}
