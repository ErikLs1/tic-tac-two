using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class JsonSavedGames : PageModel
{

    private readonly IGameRepository _gameRepository;

    public JsonSavedGames(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public string Username { get; set; } = default!;

    public IList<GameState> Games { get; set; } = new List<GameState>();

    
    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Username))
        {
            TempData["Error"] = "No username provided.";
            return RedirectToPage("./LoginPage", new { error = "No username provided." });
        }
        
        Games = _gameRepository.GetSavedGames();
        return Page();
    }
}