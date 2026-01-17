using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace Bricouli.Pages.Account
{
 public class LoginModel : PageModel
 {
 private readonly SignInManager<IdentityUser> _signInManager;
 public LoginModel(SignInManager<IdentityUser> signInManager)
 {
 _signInManager = signInManager;
 }

 [BindProperty]
 public string Email { get; set; } = string.Empty;
 [BindProperty]
 public string Password { get; set; } = string.Empty;

 public void OnGet() {}

 public async Task<IActionResult> OnPostAsync()
 {
 var result = await _signInManager.PasswordSignInAsync(Email, Password, isPersistent:false, lockoutOnFailure:false);
 if (result.Succeeded) return RedirectToPage("/Index");
 ModelState.AddModelError(string.Empty, "Échec de la connexion.");
 return Page();
 }
 }
}
