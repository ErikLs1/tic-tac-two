using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class SqlSavedGames : PageModel
{
    private readonly DAL.AppDbContext _context;

    public SqlSavedGames(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string Username { get; set; } = string.Empty;
    public IList<Game> Game { get;set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Username))
        {
            
            return RedirectToPage("./LoginPage", new { error = "No username provided." });
        }
        
        Game = await _context.Games
            .Include(g => g.Configuration).ToListAsync();

        return Page();
    }
}