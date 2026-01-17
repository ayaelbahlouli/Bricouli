using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bricouli.Data;
using Bricouli.Models;

namespace Bricouli.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProviderApplicationsController : Controller
    {
        private readonly BricoiliDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProviderApplicationsController(
            BricoiliDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _dbContext.ProviderApplications
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(new ProviderApplication());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderApplication model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;
            _dbContext.ProviderApplications.Add(model);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _dbContext.ProviderApplications.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProviderApplication model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _dbContext.ProviderApplications.Update(model);
            await _dbContext.SaveChangesAsync();

            if (string.Equals(model.Status, "approved", StringComparison.OrdinalIgnoreCase))
            {
                await EnsureProfessionalRoleAsync();
                var user = !string.IsNullOrWhiteSpace(model.UserId)
                    ? await _userManager.FindByIdAsync(model.UserId)
                    : await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (!roles.Contains("Professional"))
                    {
                        await _userManager.AddToRoleAsync(user, "Professional");
                    }
                    if (!roles.Contains("User"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task EnsureProfessionalRoleAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Professional"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Professional"));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _dbContext.ProviderApplications.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _dbContext.ProviderApplications.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _dbContext.ProviderApplications.Remove(item);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
