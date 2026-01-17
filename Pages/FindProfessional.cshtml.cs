using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bricouli.Data;
using Bricouli.Models;

namespace Bricouli.Pages
{
    public class FindProfessionalModel : PageModel
    {
        private readonly BricoiliDbContext _dbContext;

        public FindProfessionalModel(BricoiliDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ProviderApplication> Results { get; private set; } = new();
        public List<string> Categories { get; private set; } = new();
        public List<string> PopularCategories { get; private set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        public int ResultCount { get; private set; }
        public int TotalApprovedCount { get; private set; }

        public void OnGet()
        {
            var baseQuery = _dbContext.ProviderApplications.AsNoTracking()
                .Where(x => x.Status == "approved");

            if (!string.IsNullOrWhiteSpace(Category))
            {
                baseQuery = baseQuery.Where(x => x.Category == Category);
            }

            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim().ToLower();
                baseQuery = baseQuery.Where(x =>
                    x.Name.ToLower().Contains(q) ||
                    x.Category.ToLower().Contains(q) ||
                    x.Description.ToLower().Contains(q));
            }

            var query = Sort switch
            {
                "name" => baseQuery.OrderBy(x => x.Name),
                "oldest" => baseQuery.OrderBy(x => x.CreatedAt),
                _ => baseQuery.OrderByDescending(x => x.CreatedAt)
            };

            Results = query.ToList();
            ResultCount = Results.Count;

            Categories = _dbContext.ProviderApplications.AsNoTracking()
                .Where(x => x.Status == "approved")
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            PopularCategories = _dbContext.ProviderApplications.AsNoTracking()
                .Where(x => x.Status == "approved")
                .GroupBy(x => x.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(6)
                .ToList();

            TotalApprovedCount = _dbContext.ProviderApplications.AsNoTracking()
                .Count(x => x.Status == "approved");
        }
    }
}
