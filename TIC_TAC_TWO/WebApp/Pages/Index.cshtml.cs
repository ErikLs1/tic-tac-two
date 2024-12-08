using System.Runtime.InteropServices.JavaScript;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging.Signing;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    
    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }

    [BindProperty]
    public string? UserName { get; set; }
    
    [BindProperty]
    public string? Password { get; set; }
    
    [BindProperty]
    public bool IsAdmin { get; set; }
    
    public void OnGet()
    {
    }
    
    public IActionResult OnPost()
    {
        UserName = UserName?.Trim();
        Password = Password?.Trim();
        
        if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
        {
            using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            var user = context.Users.FirstOrDefault(u => u.Username == UserName && u.Password == Password);

            if (user == null)
            {
                Error = "Wrong username or password";
                return Page();
            }

            IsAdmin = user.Username == "admin" && user.Password == "admin123";
            return RedirectToPage("./Home", new {userName = UserName, isAdmin = IsAdmin});
        }

        Error = "Wrong username or password";

        return Page();
    }
}