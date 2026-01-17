using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Bricouli.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Razor Pages
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
else
{
    builder.Services.AddRazorPages();
}

// ============================================
// DATABASE - SQL Server + EF Core
// ============================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=Bricouli;Trusted_Connection=true;";

builder.Services.AddDbContext<BricoiliDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity DB (for ASP.NET Core Identity)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity (requires package Microsoft.AspNetCore.Identity.EntityFrameworkCore)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

// ============================================
// RESPONSE COMPRESSION
// ============================================
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = new[]
    {
      "application/javascript",
 "application/json",
        "text/css",
        "text/html",
        "text/plain",
        "text/xml",
        "application/xml+rss",
        "application/rss+xml"
    };
});

// ============================================
// RESPONSE CACHING
// ============================================
builder.Services.AddResponseCaching();

var app = builder.Build();

// ============================================
// DATABASE MIGRATION
// ============================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<BricoiliDbContext>();
    db.Database.Migrate();

    var appDb = services.GetRequiredService<ApplicationDbContext>();
    appDb.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    var adminRole = "Admin";
    var userRole = "User";
    var professionalRole = "Professional";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }
    if (!await roleManager.RoleExistsAsync(userRole))
    {
        await roleManager.CreateAsync(new IdentityRole(userRole));
    }
    if (!await roleManager.RoleExistsAsync(professionalRole))
    {
        await roleManager.CreateAsync(new IdentityRole(professionalRole));
    }

    var adminEmail = builder.Configuration["Admin:Email"] ?? "admin@bricouli.local";
    var adminPassword = builder.Configuration["Admin:Password"] ?? "Admin123";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}

// ============================================
// MIDDLEWARE PIPELINE
// ============================================
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files with caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".css") ||
      ctx.File.Name.EndsWith(".js") ||
            ctx.File.Name.EndsWith(".png") ||
          ctx.File.Name.EndsWith(".jpg") ||
            ctx.File.Name.EndsWith(".jpeg") ||
 ctx.File.Name.EndsWith(".gif") ||
   ctx.File.Name.EndsWith(".svg") ||
            ctx.File.Name.EndsWith(".webp"))
        {
            ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=2592000"; // 30 days
  }
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
