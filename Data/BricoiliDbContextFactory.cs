using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bricouli.Data
{
 // Design-time factory for EF Core tools
 public class BricoiliDbContextFactory : IDesignTimeDbContextFactory<BricoiliDbContext>
 {
 public BricoiliDbContext CreateDbContext(string[] args)
 {
 // Build config to read connection string from appsettings.json
 var builder = new ConfigurationBuilder()
 .SetBasePath(Directory.GetCurrentDirectory())
 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
 .AddEnvironmentVariables();

 var configuration = builder.Build();

 var connectionString = configuration.GetConnectionString("DefaultConnection")
 ?? "Server=(localdb)\\mssqllocaldb;Database=Bricouli;Trusted_Connection=true;";

 var optionsBuilder = new DbContextOptionsBuilder<BricoiliDbContext>();
 optionsBuilder.UseSqlServer(connectionString);

 return new BricoiliDbContext(optionsBuilder.Options);
 }
 }
}
