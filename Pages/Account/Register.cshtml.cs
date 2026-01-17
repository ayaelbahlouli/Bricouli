using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bricouli.Pages.Account
{
 public class RegisterModel : PageModel
 {
 private readonly UserManager<IdentityUser> _userManager;
 private readonly SignInManager<IdentityUser> _signInManager;
 public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
 {
 _userManager = userManager;
 _signInManager = signInManager;
 }

 [BindProperty]
 public string Name { get; set; } = string.Empty;
 [BindProperty]
 public string Email { get; set; } = string.Empty;
 [BindProperty]
 public string Password { get; set; } = string.Empty;

 public void OnGet() {}

 public async Task<IActionResult> OnPostAsync()
 {
 if (!ModelState.IsValid) return Page();
 var user = new IdentityUser { UserName = Email, Email = Email };
 var result = await _userManager.CreateAsync(user, Password);
 if (result.Succeeded)
 {
 if (!string.IsNullOrWhiteSpace(Name))
 {
 await _userManager.AddClaimAsync(user, new Claim("FullName", Name.Trim()));
 }
 await _userManager.AddToRoleAsync(user, "User");
 await _signInManager.SignInAsync(user, isPersistent:false);
 return RedirectToPage("/Index");
 }
 foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
 return Page();
 }
 }
}
