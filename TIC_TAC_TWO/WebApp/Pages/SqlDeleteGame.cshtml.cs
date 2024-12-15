using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class SqlDeleteGame : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public SqlDeleteGame(AppDbContext context)
    {
        _context = context;
    }


    [BindProperty]
    public Game Game { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await _context.Games.FirstOrDefaultAsync(m => m.Id == id);

        if (game is not null)
        {
            Game = game;

            return Page();
        }

        return NotFound();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            Game = game;
            _context.Games.Remove(Game);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./SqlSavedGames");
    }
}