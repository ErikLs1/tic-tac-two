using DAL;
using GameBrain;
using MenySystem;

namespace ConsoleApp;

public class GameHelpers
{
    // private static IConfigRepository _configRepository = new ConfigRepositoryJson();
    // private static IGameRepository _gameRepository = new GameRepositoryJson();
    
    private static IConfigRepository _configRepository = new ConfigRepositoryDb();
    private static IGameRepository _gameRepository = new GameRepositoryDb();

    public static void InitializeRepositories(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }
    
    public static void DisplayHeader(string title)
    {
        Console.Clear();
        Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m");
        Console.WriteLine("\x1b[1m\x1b[36m====================\x1b[0m\x1b[0m");
        Console.WriteLine(title);
    }

    private static string PromptForNonDuplicateName(string message)
    {
        Console.Write(message);
        return Console.ReadLine()?.Trim() ?? "Player";
    }

    public static (string, string) GetUsersNames()
    {
        DisplayHeader("");
        
        string player1 = PromptForNonDuplicateName("Enter 1st Player's name: ");
        string player2;

        do
        {
            player2 = PromptForNonDuplicateName("Enter 2nd Player's name: ");
            if (player2 != player1) break;
            
            Console.WriteLine("The 2nd player's name must be different from the 1st player's name. Please enter a different name.");
        } while (true);
        
        Console.Clear();
        return (player1, player2);
    }
    
    
    public static EGamePiece GetStartingPlayer() 
    {
            DisplayHeader("Who will start the game?");
            Console.WriteLine($"    1. X");
            Console.WriteLine($"    2. O");
            
            do
            {
                Console.Write("Insert 1 or 2 to choose who starts: ");
                var startChoice = Console.ReadLine()?.Trim();
            
                if (startChoice == "1") return EGamePiece.X;
                if (startChoice == "2") return EGamePiece.O;

                Console.WriteLine("Invalid choice! Please enter '1' for the first player or '2' for the second player.");

            } while (true);
    }

    public static (string, string) GetPlayersSymbol(string playerX, string playerO)
    {
        DisplayHeader("");
        string symbolX = PromptForSymbol($"{playerX} (default is 'X')", "X");
        string symbolO;

        do
        {
            symbolO = PromptForSymbol($"{playerO} (default is 'O')", "O");
            if (symbolO != symbolX) break;
            
            Console.WriteLine("The symbols for each player must be different.");

        } while (true);

        return (symbolX, symbolO);
    }

    private static string PromptForSymbol(string player, string defaultSymbol)
    {
        string symbol;
        do
        {
            Console.Write($"Enter the symbol for {player}: ");
            symbol = Console.ReadLine()?.Trim()!;

            if (string.IsNullOrEmpty(symbol))
            {
                return defaultSymbol;
            }

            if (symbol.Length is 1 or 2)
            {
                return symbol;
            }

            Console.WriteLine("Wrong input. Only one character is allowed.");
        } while (true);
    }
    
    public static GameConfiguration SelectConfiguration()
    {
        int chosenConfigIndex = ChooseConfiguration();
        var configNames = ConfigRepositoryHardcoded.GetConfigurationNames();
        return chosenConfigIndex >= configNames.Count
            ? ConfigRepositoryHardcoded.ConfigureCustomGame()
            : ConfigRepositoryHardcoded.GetConfigurationByName(configNames[chosenConfigIndex]);
    }

    private static int ChooseConfiguration()
    {
        var configMenuItems = ConfigRepositoryHardcoded.GetConfigurationNames()
            .Select((name, index) => new MenuItem
            {
                Title = name,
                Shortcut = (index + 1).ToString(),
                MenuItemAction = () => index.ToString()
            })
            .ToList();
        
        configMenuItems.Add(new MenuItem
        {
            Title = "Custom Configuration",
            Shortcut = (configMenuItems.Count + 1).ToString(),
            MenuItemAction = () => configMenuItems.Count.ToString()
        });
        
        var configMenu = new Menu(EMenuLevel.Secondary,
            "\x1b[1m\x1b[35mTIC-TAC-TWO - choose configuration\x1b[0m\x1b[0m",
            configMenuItems,
            isCustomMenu: true
        );
        
        return int.Parse(configMenu.Run());
    }

    public static string GetPlayerChoice()
    {
        Console.WriteLine("Choose what you want to do:");
        Console.WriteLine("\x1b[1m\x1b[35mA)\x1b[0m\x1b[0m Make a move");
        Console.WriteLine("\x1b[1m\x1b[35mB)\x1b[0m\x1b[0m Move the grid");
        Console.WriteLine("\x1b[1m\x1b[35mC)\x1b[0m\x1b[0m Move the piece");

        string choice;
        do
        {
            Console.Write("I choose: ");
            choice = Console.ReadLine()?.Trim().ToUpper()!;
            if (choice == "A" || choice == "B" || choice == "C") return choice;
            
            Console.WriteLine("Invalid choice. Please select A, B or C.");
        } while (true);
    }

    public static void HandlePlayerChoice(string choice, TicTacTwoBrain gameInstance)
    {
        switch (choice)
        {
            case "A":
                ExecuteMove(gameInstance);
                break;
            
            case "B":
                ExecuteGridMove(gameInstance);
                break;
            
            case "C":
                MovePiece(gameInstance, gameInstance._nextMoveBy);
                break;
        }
    }

    public static void ExecuteMove(TicTacTwoBrain gameInstance)
    {
        var (inputX, inputY) = InsertCoordinates(gameInstance);
        if (!gameInstance.MakeAMove(inputX, inputY))
        {
            Console.WriteLine("Wrong input!");
            return;
        }

        var winner = gameInstance.CheckForWin(inputX, inputY);
        if (winner != null)
        {
            Console.Clear();
            DisplayHeader("We have a winner!");
            Console.WriteLine($"{gameInstance.GetCurrentPlayer()} has won the game! \ud83e\udd73\ud83e\udd73\ud83e\udd73\ud83e\udd73 ");
            Console.WriteLine();
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Environment.Exit(0);
        }
    }

    private static void ExecuteGridMove(TicTacTwoBrain gameInstance)
    {
        string move;
        bool isValidMove = false;
        
        do
        {
            Console.Write("Enter your move (up, down, left, right, up-left, up-right, down-left, down-right): ");
            move = Console.ReadLine()?.ToLower()!;
            isValidMove = MoveGrid(gameInstance, move);
            if (!isValidMove)
            {
                Console.WriteLine("Wrong input.Please try again.");
            }
        } while (!isValidMove);
        
        gameInstance. _nextMoveBy = gameInstance._nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
    }
    
    private static bool MoveGrid(TicTacTwoBrain gameInstance, string direction)
    {
        return direction switch
        {
            "up" => gameInstance.CanMoveGrid(0, -1),
            "down" => gameInstance.CanMoveGrid(0, 1),
            "left" => gameInstance.CanMoveGrid(-1, 0),
            "right" => gameInstance.CanMoveGrid(1, 0),
            "up-left" => gameInstance.CanMoveGrid(-1, -1),
            "up-right" => gameInstance.CanMoveGrid(1, -1),
            "down-left" => gameInstance.CanMoveGrid(-1, 1),
            "down-right" => gameInstance.CanMoveGrid(1, 1),
            _ => false
        };
    }

    private static (int, int) InsertCoordinates(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine($"{gameInstance.GetCurrentPlayer()}'s turn to make a move");
        Console.Write("Give me coordinates <\x1b[1m\x1b[31mX\x1b[0m\x1b[0m,\x1b[1m\x1b[32mY\x1b[0m\x1b[0m> or save:");
    
       
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (input?.ToLower() == "save")
            {
                SaveGameAndExit(gameInstance);
            }
            
            if (ParseCoordinates(input, out var x, out var y)
                && IsWithinBounds(gameInstance, x, y)
                && gameInstance.GetPiece(x, y) == EGamePiece.Empty)
            {
                return (x, y);
            }
            
            Console.WriteLine("Invalid or occupied position. Try again.");
        }
    }

    private static void SaveGameAndExit(TicTacTwoBrain gameInstance)
    {
        Console.Write("Enter a name for your game: ");
        var saveName = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(saveName))
        {
            saveName = "GameSave";
        }

        var gameState = gameInstance.GetGameState();
        _gameRepository.SaveGame(gameState, saveName);
        
        Console.WriteLine("Game has been saved. Exiting the game...");
        Environment.Exit(0);
    }

    private static bool ParseCoordinates(string? input, out int x, out int y)
    {
        x = y = -1;
        var parts = input?.Split(",");
        return parts != null && 
               parts.Length == 2 && int.TryParse(parts[0], out x) && 
               int.TryParse(parts[1], out y);
    }

    private static bool IsWithinBounds(TicTacTwoBrain gameInstance, int x, int y)
    {
        int boardWidth = gameInstance.BoardWidth;
        int boardHeight = gameInstance.BoardWidth;

        return x >= 0 && x < boardWidth && y >= 0 && y < boardHeight;
    }
    private static bool IsWithinBoundsGrid(TicTacTwoBrain gameInstance, int x, int y)
    {
        var gridPosition = gameInstance.GridPosition;

        int gridLeft = gridPosition.x;
        int gridTop = gridPosition.y;
        int gridRight = gridLeft + gameInstance.GridWidth;
        int gridBottom = gridTop + gameInstance.GridHeight;
        // Console.WriteLine($"Grid bounds: Left={gridLeft}, Right={gridRight}, Top={gridTop}, Bottom={gridBottom}");
        return x >= gridLeft && x < gridRight && y >= gridTop && y < gridBottom;
    }
    
    private static void MovePiece(TicTacTwoBrain gameInstance, EGamePiece gamePiece)
    {
        while (true)
        {
            Console.Write($"It's {gamePiece}'s turn. Enter the coordinates of the piece you want to move <x,y>: ");
            var startInput = Console.ReadLine()!;
            var startInputSplit = startInput.Split(",");
    
            if (startInputSplit.Length != 2 || 
                !int.TryParse(startInputSplit[0], out var startX) ||
                !int.TryParse(startInputSplit[1], out var startY))
            {
                Console.WriteLine("Wrong input. PLease, make sure coordinates are in the correct format.");
                continue;
            }
            if (!IsWithinBoundsGrid(gameInstance, startX, startY))
            {
                Console.WriteLine("The selected piece is outside the grid. Please choose a piece within the grid.");
                continue;
            }
            
            if (gameInstance.GetPiece(startX, startY) != gamePiece)
            {
                Console.WriteLine($"You can only move your own piece ({gamePiece}). Please, choose another piece.");
                continue;
            }
            
            Console.Write("Enter the coordinates where you want to move your piece <x,y>: ");
            var targetInput = Console.ReadLine()!;
            var targetInputSplit = targetInput.Split(",");
    
            if (targetInputSplit.Length != 2 || !int.TryParse(targetInputSplit[0], out var targetX) ||
                !int.TryParse(targetInputSplit[1], out var targetY))
            {
                Console.WriteLine("Wrong input. Please, make sure the coordinates are in the correct format. ");
                continue;
            }
            
            if (!IsWithinBoundsGrid(gameInstance, targetX, targetY))
            {
                Console.WriteLine("The piece can only be moved within the grid. Please choose another position.");
                continue;
            }
            
            if (gameInstance.GetPiece(targetX, targetY) != EGamePiece.Empty)
            {
                Console.WriteLine("This spot is occupied. Chose another place.");
                continue;
            }
            
            Console.WriteLine($"Moved piece from ({startX},{startY}) to ({targetX},{targetY}).");
            gameInstance.MoveAPiece(startX, startY,targetX,targetY);
            break;
        }
    }
}