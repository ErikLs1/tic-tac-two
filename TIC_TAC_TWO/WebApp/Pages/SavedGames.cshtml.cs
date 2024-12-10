using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class SavedGames : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? UserName { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool IsAdmin { get; set; }
    
    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            RedirectToPage("./Index", new { error = "No username provided" });
        }
    }
}