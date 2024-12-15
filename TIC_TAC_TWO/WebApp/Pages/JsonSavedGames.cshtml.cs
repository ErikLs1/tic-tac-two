using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class JsonSavedGames : PageModel
{

    private readonly IGameRepository _gameRepository;

    public JsonSavedGames(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IList<GameState> Games { get; set; } = new List<GameState>();
        
    public async Task OnGetAsync()
    {
        Games = _gameRepository.GetSavedGames();
    }
}