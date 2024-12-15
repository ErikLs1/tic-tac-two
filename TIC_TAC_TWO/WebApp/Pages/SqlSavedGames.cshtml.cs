using DAL;
using Domain;
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


    public IList<Game> Game { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Game = await _context.Games
            .Include(g => g.Configuration).ToListAsync();
    }
}