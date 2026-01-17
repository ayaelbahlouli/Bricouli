using Microsoft.AspNetCore.Mvc;
using Bricouli.Models;
using Bricouli.Data;

namespace Bricouli.Controllers
{
    public class ContactController : Controller
    {
        private readonly BricoiliDbContext _dbContext;

        public ContactController(BricoiliDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var message = new ContactMessage
            {
                Name = form.Name,
                Email = form.Email,
                Message = form.Message
            };

            _dbContext.ContactMessages.Add(message);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = "Votre message a bien ete envoye !";

            return RedirectToAction("Index");
        }
    }
}
