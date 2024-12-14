    using DAL;
    using GameBrain;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    namespace WebApp.Pages;

    public class PlayGame : PageModel
    {
        private readonly IConfigRepository _configRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<PlayGame> _logger;
        public PlayGame(IConfigRepository configRepository, IGameRepository gameRepository, ILogger<PlayGame> logger)
        {
            _configRepository = configRepository;
            _gameRepository = gameRepository;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)] public int GameId { get; set; } = default!;

        [BindProperty(SupportsGet = true)] public int ConfigId { get; set; } = default!;

        [BindProperty] public int Y { get; set; } = default!;

        [BindProperty] public int X { get; set; } = default!;

        [BindProperty(SupportsGet = true)] public string Action { get; set; } = string.Empty;
        
        [BindProperty] public string Direction { get; set; } = string.Empty;
        
        [BindProperty(SupportsGet = true)]
        public bool IsActionInProgress { get; set; } = false;
        
        [BindProperty(SupportsGet = true)]
        public int? SelectedPieceX { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public int? SelectedPieceY { get; set; } = default!;
        
        public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
        public bool IsGameOver { get; set; } = false;
        private string Winner { get; set; } = string.Empty;

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
            
            // PLACING A PIECE WHEN ACTION IS MAKE A MOVE
            if (!string.IsNullOrEmpty(Action) && Action == "Make-a-Move" && IsActionInProgress && string.IsNullOrEmpty(Direction))
            {
                _logger.LogInformation("Attempting to make a move at X={X}, Y={Y} under Make-a-Move action.", X, Y);
                bool moveResult = TicTacTwoBrain.MakeAMove(X, Y);
                    
                if (moveResult)
                {
                    _gameRepository.SaveGame(TicTacTwoBrain.GetGameState(), TicTacTwoBrain.GetGameConfigName());
                    Action = string.Empty;
                    IsActionInProgress = false;
                    return RedirectToPage("./PlayGame", new { GameId = GameId });
                }
                else
                {
                    Message = "Invalid move.Please try again.";
                    return Page();
                }
            }
            // HANDLE PIECE MOVEMENT
            else if (!string.IsNullOrEmpty(Action) && Action == "Move-the-Piece" && IsActionInProgress && string.IsNullOrEmpty(Direction))
            {
                if (SelectedPieceX == null || SelectedPieceY == null)
                {
                    var piece = TicTacTwoBrain.GetPiece(X, Y);
                    if (piece == TicTacTwoBrain.GetCurrentPlayer() &&
                        TicTacTwoBrain.IsWithinBoundsGrid(TicTacTwoBrain, X, Y))
                    {
                        SelectedPieceX = X;
                        SelectedPieceY = Y;
                        Message = "Piece selected. Now choose where to move it.";
                        return Page();
                    }
                    else
                    {
                        Message = "Invalid piece. Choose another one.";
                        return Page();
                    }
                }
                else
                {
                    if (TicTacTwoBrain.IsWithinBoundsGrid(TicTacTwoBrain, X, Y) &&
                        TicTacTwoBrain.GetPiece(X, Y) == EGamePiece.Empty)
                    {
                        TicTacTwoBrain.MoveAPiece(SelectedPieceX.Value, SelectedPieceY.Value, X, Y);
                        _gameRepository.SaveGame(TicTacTwoBrain.GetGameState(),TicTacTwoBrain.GetGameConfigName());
                        Action = string.Empty;
                        IsActionInProgress = false;
                        SelectedPieceX = null;
                        SelectedPieceY = null;
                        return RedirectToPage("./PlayGame", new { GameId = GameId });
                    }
                    else
                    {
                        Message = "Invalid target cell. Choose another one";
                        return Page();
                    }
                }
            } 
            // HANDLE GRID MOVEMENT
            else if (!string.IsNullOrEmpty(Direction))
            {
                
                bool moveSuccess = TicTacTwoBrain.MoveGrid(TicTacTwoBrain, Direction.ToLower());
               
                if (moveSuccess)
                {
                    _gameRepository.SaveGame(TicTacTwoBrain.GetGameState(),TicTacTwoBrain.GetGameConfigName());
                    Message = $"Grid moved {Direction} successfully.";
                }
                else
                {
                    Message = $"Cannot move grid {Direction}. Please choose a different direction.";
                }
                return RedirectToPage("./PlayGame", new { GameId = GameId });
            }
            // HANDLE ACTIONS
            else if (!string.IsNullOrEmpty(Action) && string.IsNullOrEmpty(Direction))
            {
                switch (Action)
                {
                    
                    case "Make-a-Move":
                        IsActionInProgress = true;
                        return Page();
                    case "Move-the-Grid":
                        IsActionInProgress = true;
                        return Page();
                    case "Move-the-Piece":
                        IsActionInProgress = true;
                        return Page();
                    default:
                        TempData["Error"] = "Unknown action selected.";
                        return RedirectToPage("./PlayGame", new { GameId = GameId });
                }
            } 
            // NORMAL MOVES
            else
            {
                 
                if (dbGame.MoveCount >= dbGame.GameConfiguration.MovePieceAfterNMoves)
                {
                    IsActionInProgress = true;
                    return Page();
                }
                else
                {
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
                }
                
            }
            
            return Page();
        }
    }