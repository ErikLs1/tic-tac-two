using DAL;
using GameBrain;
using MenySystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly ConfigRepository ConfigRepository = new ConfigRepository();

    public static string MainLoop()
    {
        Console.Write("Enter Player X's name: ");
        var playerX = Console.ReadLine() ?? "Player X";
        
        Console.Write("Enter Player O's name: ");
        var playerO = Console.ReadLine() ?? "Player O";

        EGamePiece startingPlayer = GetStartingPlayer();
        
        var chosenConfigShortcut = ChooseConfiguration();

        // if (!int.TryParse(chosenConfigShortcut, out var configNo))
        // {
        //     return chosenConfigShortcut;
        // }

        GameConfiguration chosenConfig;
        
        if (chosenConfigShortcut == "custom")
        {
            chosenConfig = ConfigRepository.ConfigureCustomGame();
        }
        else
        {
            chosenConfig = ConfigRepository.GetConfigurationByName(
                ConfigRepository.GetConfigurationNames()[int.Parse(chosenConfigShortcut)]);
        }
        

        var gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO, startingPlayer);
        var moves = 0;
        
        do
        {
            Console.Clear();
            Console.WriteLine("TIC-TAC-TWO");
            Console.WriteLine(new string('=', gameInstance.DimX * 4));
            Console.WriteLine();
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Console.WriteLine();
            
            if (moves >= 4)
            {
                Console.WriteLine("Choose what you want to do:");
                Console.WriteLine("A) Make a move");
                Console.WriteLine("B) Move the grid");
                Console.WriteLine("C) Move the piece");
                string choice;
                do
                {
                    Console.Write("I choose:");
                    choice = Console.ReadLine()?.ToUpper()!;
                    if (choice != "A" && choice != "B" && choice != "C")
                    {
                        Console.WriteLine("Invalid choice. Please select A, B, or C.");
                    }
                } while (choice != "A" && choice != "B" && choice != "C");

                switch (choice)
                {
                    case "A":
                        var (inputX, inputY) = InsertCoordinates(gameInstance);
                        if (!gameInstance.MakeAMove(inputX, inputY))
                        {
                            Console.WriteLine("Wrong Input.");
                        }
                        else
                        {
                            var winner = gameInstance.CheckForWin(inputX, inputY);
                            if (winner != null)
                            {
                                Console.Clear();
                                Console.WriteLine("TIC-TAC-TWO");
                                Console.WriteLine(new string('=', gameInstance.DimX * 4));
                                Console.WriteLine();
                                Console.WriteLine($"{winner} has won the game!\ud83e\udd73\ud83e\udd73");
                                ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
                                return "game over!";
                            }
                        }
                        moves++;
                        Console.Clear();
                        break;

                    case "B":
                        Console.Write("Enter your move (up, down, left, right, up-left, up-right, down-left, down-right): ");
                        string move = Console.ReadLine()?.ToLower()!;
                        Console.WriteLine();
                        MoveGrid(gameInstance, move);
                        gameInstance. _nextMoveBy = gameInstance._nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                        Console.Clear();
                        break;

                    case "C":
                        MovePiece(gameInstance, gameInstance._nextMoveBy);
                        Console.Clear();
                        break;
                    
                }
            }
            else
            {
                var (inputX, inputY) = InsertCoordinates(gameInstance);
                gameInstance.MakeAMove(inputX, inputY);
                moves++;
                Console.Clear();
            }
            
        } while (true);

        // return "Good Job PlayerName";
    }

    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = ConfigRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }

        configMenuItems.Add(new MenuItem()
        {
            Title = "Custom Configuration",
            Shortcut = (configMenuItems.Count + 1).ToString(),
            MenuItemAction = () => "custom"
        });
        
        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        return configMenu.Run();
    }
    
    private static bool MoveGrid(TicTacTwoBrain gameInstance, string direction)
    {
        switch (direction)
        {
            case "up":
                return gameInstance.CanMoveGrid(0, -1);
            case "down":
                return gameInstance.CanMoveGrid(0, 1);
            case "left":
                return gameInstance.CanMoveGrid(-1, 0);
            case "right":
                return gameInstance.CanMoveGrid(1, 0);
            case "up-left":
                return gameInstance.CanMoveGrid(-1, -1);
            case "up-right":
                return gameInstance.CanMoveGrid(1, -1);
            case "down-left":
                return gameInstance.CanMoveGrid(-1, 1);
            case "down-right":
                return gameInstance.CanMoveGrid(1, 1);
            default:
                return false;
        }
    }

    private static (int, int) InsertCoordinates(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine($"{gameInstance.GetCurrentPlayer()}'s ");
        Console.Write("Give me coordinates <x,y> or save:");
    
       
        while (true)
        {
            var input = Console.ReadLine()!;
            var inputSplit = input.Split(",");
    
            if (inputSplit.Length != 2)
            {
                Console.Write("Wrong input. Please make sure you input coordinates correctly <x,y>: ");
                continue;
            }
    
            if (!int.TryParse(inputSplit[0], out var inputX) || !int.TryParse(inputSplit[1], out var inputY))
            {
                Console.Write("Wrong coordinates. Please, make sure both x and y are numbers: ");
                continue;
            }
    
            if (gameInstance.GetPiece(inputX, inputY) != EGamePiece.Empty)
            {
                Console.Write("This spot is occupied. Please, choose different position <x,y>: ");
                continue;
            }
            return(inputX, inputY);
        }
    }

    private static EGamePiece GetStartingPlayer() 
    {
        while (true)
        {
            Console.Write("Who will start (X/0): ");
            var startChoice = Console.ReadLine()?.Trim().ToUpper();
            if (startChoice == "X") return EGamePiece.X;
            if (startChoice == "O") return EGamePiece.O;

            Console.WriteLine("Invalid choice! Please enter 'X' or 'O'.");
        }
    }
    private static void MovePiece(TicTacTwoBrain gameInstance, EGamePiece gamePiece)
    {
        while (true)
        {
            Console.Write($"It's {gamePiece}'s turn. Enter the coordinates of the piece you want to move <x,y>: ");
            var startInput = Console.ReadLine()!;
            var startInputSplit = startInput.Split(",");
    
            if (startInputSplit.Length != 2 || !int.TryParse(startInputSplit[0], out var startX) ||
                                                !int.TryParse(startInputSplit[1], out var startY))
            {
                Console.WriteLine("Wrong input. PLease, make sure coordinates are in the correct format.");
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