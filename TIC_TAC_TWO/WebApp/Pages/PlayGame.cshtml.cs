using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGame(IGameRepository gameRepository, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }

    [BindProperty(SupportsGet = true)] 
    public int GameId { get; set; } = default!;

    [BindProperty(SupportsGet = true)] 
    public EGamePiece NextMoveBy { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] 
    public int ConfigId { get; set; }

    public TicTacTwoBrain TicTacToeBrain { get; set; } = default!;

    public void OnGet(int? x, int? y)
    {
    }
}