using Microsoft.EntityFrameworkCore;
using Bricouli.Models;

namespace Bricouli.Data
{
    public class BricoiliDbContext : DbContext
    {
        public BricoiliDbContext(DbContextOptions<BricoiliDbContext> options) : base(options)
        {
        }

        public DbSet<DevisRequest> DevisRequests { get; set; } = null!;
        public DbSet<ContactMessage> ContactMessages { get; set; } = null!;
        public DbSet<ProviderApplication> ProviderApplications { get; set; } = null!;

      protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
            base.OnModelCreating(modelBuilder);

        // Configure DevisRequest
       modelBuilder.Entity<DevisRequest>()
         .HasKey(d => d.Id);

            modelBuilder.Entity<DevisRequest>()
     .Property(d => d.CreatedAt)
    .HasDefaultValueSql("GETUTCDATE()");

     modelBuilder.Entity<DevisRequest>()
           .Property(d => d.Status)
         .HasDefaultValue("pending");

        // Configure ContactMessage
        modelBuilder.Entity<ContactMessage>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<ContactMessage>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Configure ProviderApplication
        modelBuilder.Entity<ProviderApplication>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<ProviderApplication>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<ProviderApplication>()
            .Property(p => p.Status)
            .HasDefaultValue("new");

        }
    }
}
