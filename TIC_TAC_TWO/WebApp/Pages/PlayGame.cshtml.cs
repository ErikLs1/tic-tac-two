using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGame(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public int GameId { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public int ConfigId { get; set; } = default!;

    [BindProperty] public int Y { get; set; } = default!;

    [BindProperty] public int X { get; set; } = default!;

    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
    public string Message { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (GameId > 0)
        {
            var dbGame = _gameRepository.LoadGame(GameId);
            TicTacTwoBrain = new TicTacTwoBrain(dbGame);
            return Page();
        }
        else if (ConfigId > 0)
        {
            var dbConfig = _configRepository.GetConfigurationById(ConfigId);
            TicTacTwoBrain = new TicTacTwoBrain(dbConfig, EGamePiece.X);
            var gameState = TicTacTwoBrain.GetGameState();
            _gameRepository.SaveGame(gameState, TicTacTwoBrain.GetGameConfigName());
            GameId = (int)gameState.GameId!;

            return RedirectToPage("./PlayGame", new { GameId = GameId });
        }
        else
        {
            TempData["Error"] = "No GameId or ConfigId provided.";
            return RedirectToPage("./Index");
        }
    }

    public IActionResult OnPost()
    {
        var dbGame = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(dbGame);

        bool moveResult = TicTacTwoBrain.MakeAMove(X, Y);
        
        if (moveResult)
        {
            _gameRepository.SaveGame(TicTacTwoBrain.GetGameState(), TicTacTwoBrain.GetGameConfigName());
            return RedirectToPage("./PlayGame", new { GameId = GameId });
        }
        else
        {
            Message = "Invalid move.Please try again.";
        }

        return Page();
    }
}