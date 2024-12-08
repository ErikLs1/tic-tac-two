using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? UserName { get; set; }
    
        [BindProperty(SupportsGet = true)]
        public bool IsAdmin { get; set; }
        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                RedirectToPage("./Index", new { error = "No username provided" });
            }

            ViewData["UserName"] = UserName;
            ViewData["IsAdmin"] = IsAdmin;
            
            User = await _context.Users.ToListAsync();
        }
    }
}
