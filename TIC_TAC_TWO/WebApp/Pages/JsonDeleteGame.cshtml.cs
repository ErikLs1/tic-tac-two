using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class JsonDeleteGame : PageModel
{
    private readonly IGameRepository _gameRepository;

    public JsonDeleteGame(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int GameId { get; set; }
    
    public GameState? GameToDelete { get; set; }
    
    public IActionResult OnGet(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        try
        {
            GameToDelete = _gameRepository.LoadGame(id);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToPage("./Index");
        }
    }

    public IActionResult OnPost(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        try
        {
            _gameRepository.DeleteGame(id);
            return RedirectToPage("./JsonSavedGames");
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., game not found)
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}