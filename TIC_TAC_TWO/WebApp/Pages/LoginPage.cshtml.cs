using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp;

public class LoginPage : PageModel
{
    private readonly ILogger<LoginPage> _logger;

    public LoginPage(ILogger<LoginPage> logger)
    {
        _logger = logger;
    }

    private static readonly List<User> _users = new List<User>()
    {
        new User{Username = "user1", Password = "123"},
        new User{Username = "user2", Password = "123"},
        new User{Username = "admin", Password = "123"},
    };
    
    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? UserName { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Password { get; set; }
    public void OnGet()
    {
        
    }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var inputUsername = UserName?.Trim();
        var inputPassword = Password?.Trim();

        var user = _users.FirstOrDefault(u =>
            u.Username == inputUsername &&
            u.Password == inputPassword);

        if (user != null)
        {
            return RedirectToPage("./Home", new { UserName });
        }
        else
        {
            Error = "Invalid username or password";
            return Page();
        }

    }
}