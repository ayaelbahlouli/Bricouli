using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bricouli.Data;

namespace Bricouli.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly BricoiliDbContext _dbContext;

        public DashboardController(BricoiliDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var devisCount = await _dbContext.DevisRequests.CountAsync();
            var providerCount = await _dbContext.ProviderApplications.CountAsync();
            var messageCount = await _dbContext.ContactMessages.CountAsync();

            ViewData["DevisCount"] = devisCount;
            ViewData["ProviderCount"] = providerCount;
            ViewData["MessageCount"] = messageCount;

            return View();
        }
    }
}
