using System.Diagnostics;
using Bricouli.Models;
using Bricouli.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bricouli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BricoiliDbContext _dbContext;

        public HomeController(
            ILogger<HomeController> logger,
            BricoiliDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ============================================
        // DEVIS PAGE - GET
        // ============================================
        [HttpGet("/Devis")]
        [Authorize]
        public IActionResult Devis()
        {
            return View("~/Views/Devis/Index.cshtml", new DevisRequest());
        }

        // ============================================
        // DEVIS PAGE - POST
        // ============================================
        [HttpPost("/Devis")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Devis(DevisRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Devis/Index.cshtml", model);
            }

            try
            {
                model.CreatedAt = DateTime.UtcNow;
                model.Status = "pending";
                _dbContext.DevisRequests.Add(model);
                await _dbContext.SaveChangesAsync();
                ViewBag.Success = true;
                return View("~/Views/Devis/Index.cshtml", new DevisRequest());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la soumission du devis: {ex.Message}", ex);
                ModelState.AddModelError("", "Erreur serveur lors du traitement de votre demande");
                return View("~/Views/Devis/Index.cshtml", model);
            }
        }

        // ============================================
        // ADMIN - Voir tous les devis (pour test)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> GetAllDevis()
        {
            try
            {
                var devis = await _dbContext.DevisRequests.ToListAsync();

                return Ok(new
                {
                    count = devis.Count,
                    devis = devis
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des devis: {ex.Message}");
                return StatusCode(500, new { message = "Erreur serveur", success = false });
            }
        }
    }
}
