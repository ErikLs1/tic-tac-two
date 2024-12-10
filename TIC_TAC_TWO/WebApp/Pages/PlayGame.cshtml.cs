using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly ILogger<PlayGame> _logger;

    public PlayGame(IConfigRepository configRepository, ILogger<PlayGame> logger)
    {
        _configRepository = configRepository;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)] public string ConfigName { get; set; } = default!;

    public TicTacTwoBrain? GameBrain { get; set; }

    [BindProperty] public string? Move { get; set; }

    [BindProperty] public string? Action { get; set; }

    [BindProperty] public string? Direction { get; set; }

    [BindProperty] public string? StartPosition { get; set; }

    [BindProperty] public string? TargetPosition { get; set; }

    public bool GameOver { get; set; } = false;
    public string CurrentPlayer => GameBrain != null && !GameOver
        ? (GameBrain._nextMoveBy == EGamePiece.X ? "X's Turn" : "O's Turn")
        : "";
    
    public void OnGet()
    {
        if (TempData["GameState"] != null)
        {
            var gameStateJson = TempData["GameState"] as string;
            var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson!);
            GameBrain = new TicTacTwoBrain(gameState);

            CheckForWinner();
            
            TempData.Keep("GameState");
            TempData.Keep("Action");
            TempData.Keep("SelectedPiece");
        }
        else if (!string.IsNullOrEmpty(ConfigName))
        {
            var config = _configRepository.GetConfigurationByName(ConfigName);

            if (config.BoardSizeWidth <= 0 || config.BoardSizeHeight <= 0)
            {
                TempData["Message"] = "Invalid board size in configuration.";
                return;
            }

            var initialGameState = new GameState(
                gameBoard: InitializeEmptyBoard(config.BoardSizeWidth, config.BoardSizeHeight),
                gameConfiguration: config)
            {
                GridPositionX = (config.BoardSizeWidth - config.GridWidth) / 2,
                GridPositionY = (config.BoardSizeHeight - config.GridHeight) / 2,
                NextMoveBy = EGamePiece.X
            };
            GameBrain = new TicTacTwoBrain(initialGameState);

            TempData["GameState"] = JsonSerializer.Serialize(GameBrain.GetGameState());
            TempData["Action"] = "MakeMove";
            TempData.Keep("GameState");
            TempData.Keep("Action");
        }

        CheckForWinner();
    }

    public IActionResult OnPost()
    {
        if (TempData["GameState"] == null)
        {
            TempData["Message"] = "Game state not found. Please restart the game.";
            return RedirectToPage(new { ConfigName });
        }

        // Restore Game State
        var gameStateJson = TempData["GameState"] as string;
        var gameState = JsonSerializer.Deserialize<GameState>(gameStateJson!);
        GameBrain = new TicTacTwoBrain(gameState);

        var currentAction = TempData["Action"] as string ?? "MakeMove";
        if (!string.IsNullOrEmpty(Action))
        {
            currentAction = Action;
            TempData["Action"] = currentAction;
        }
        
        if (CheckForWinner())
        {
            TempData.Keep("GameState");
            TempData.Keep("Action");
            return RedirectToPage(new { ConfigName });
        }

        bool actionDone = false;
        
        if (!string.IsNullOrEmpty(Move))
        {
            // Handle clicks on cells depending on current action
            HandleCellClick(Move, currentAction);
        }
        else if (!string.IsNullOrEmpty(Direction) && currentAction == "MoveGrid")
        {
            var result = TicTacTwoBrain.MoveGrid(GameBrain, Direction);
            if (!result)
            {
                TempData["Message"] = "Cannot move the grid in that direction. Choose another direction.";
            }
            else
            {
                TempData["Message"] = "Grid moved successfully.";
                actionDone = true;

            }
        }
        else if (currentAction == "PerformPieceMove")
        {
            HandlePieceMovement();
            TempData["Action"] = "MakeMove";
        }
        
        if (!CheckForWinner() && actionDone)
        {
            TempData["Action"] = "MakeMove";
            if (GameBrain.MoveCount >= 4)
            {
                TempData["ShowOptions"] = true;
            }
        }

        CheckForWinner();
        TempData["GameState"] = JsonSerializer.Serialize(GameBrain.GetGameState());
        TempData.Keep("GameState");
        TempData.Keep("Action");
        TempData.Keep("SelectedPiece");

        return RedirectToPage(new { ConfigName });
    }

    private void HandleCellClick(string move, string currentAction)
        {
            var coordinates = move.Split(',');
            if (coordinates.Length == 2 && int.TryParse(coordinates[0], out var x) && int.TryParse(coordinates[1], out var y))
            {
                if (currentAction == "MakeMove")
                {
                    var moveResult = GameBrain.MakeAMove(x, y);

                    if (!moveResult)
                    {
                        TempData["Message"] = "Invalid move! Please select an empty cell.";
                    }
                    else
                    {
                        if (GameBrain.MoveCount >= 4 && !CheckForWinner())
                        {
                            TempData["ShowOptions"] = true;
                        }
                    }
                }
                else if (currentAction == "MovePiece")
                {
                    // First click: select the piece start position
                    // Check if the cell clicked belongs to current player and is within grid
                    if (!TicTacTwoBrain.IsWithinBoundsGrid(GameBrain, x, y))
                    {
                        TempData["Message"] = "Please select a piece within the grid.";
                        return;
                    }
                    
                    var cellPiece = GameBrain.GetPiece(x,y);
                    if (cellPiece != GameBrain._nextMoveBy)
                    {
                        TempData["Message"] = "You can only move your own piece.";
                        return;
                    }

                    // Store selected piece
                    TempData["SelectedPiece"] = $"{x},{y}";
                    // Next action: MovePieceTarget to select target cell
                    TempData["Action"] = "MovePieceTarget";
                    TempData["Message"] = "Piece selected. Now choose an empty cell within the grid to place it.";
                }
                else if (currentAction == "MovePieceTarget")
                {
                    // Second click: place the piece in target cell
                    // Retrieve the selected piece start coordinates
                    var selectedPiece = TempData["SelectedPiece"] as string;
                    if (selectedPiece == null)
                    {
                        TempData["Message"] = "No piece selected. Please select a piece first.";
                        TempData["Action"] = "MovePiece";
                        return;
                    }
                    var startParts = selectedPiece.Split(',');
                    if (startParts.Length == 2 && int.TryParse(startParts[0], out var sx) && int.TryParse(startParts[1], out var sy))
                    {
                        // Check target cell empty and in grid
                        if (!TicTacTwoBrain.IsWithinBoundsGrid(GameBrain, x, y))
                        {
                            TempData["Message"] = "Target cell is outside the grid.";
                            return;
                        }

                        if (GameBrain.GetPiece(x,y) != EGamePiece.Empty)
                        {
                            TempData["Message"] = "Target cell is not empty.";
                            return;
                        }

                        GameBrain.MoveAPiece(sx, sy, x, y);
                        TempData["Message"] = "Piece moved successfully.";
                        // Return to MakeMove
                        TempData["Action"] = "MakeMove";
                        TempData.Remove("SelectedPiece");
                    }
                    else
                    {
                        TempData["Message"] = "Invalid selected piece coordinates.";
                        TempData["Action"] = "MovePiece";
                    }
                }
            }
            else
            {
                TempData["Message"] = "Invalid coordinates format.";
            }
        }

        private void HandlePieceMovement()
        {
            TempData["Message"] = "Piece movement is handled by clicking cells in 'MovePiece' mode, not by typed coordinates.";
        }

        private bool CheckForWinner()
        {
            return false;
        }

    private EGamePiece[][] InitializeEmptyBoard(int width, int height)
    {
        var board = new EGamePiece[width][];
        for (int x = 0; x < width; x++)
        {
            board[x] = new EGamePiece[height];
            for (int y = 0; y < height; y++)
            {
                board[x][y] = EGamePiece.Empty;
            }
        }

        return board;
    }
}