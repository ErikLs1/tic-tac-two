using ConsoleUI;
using DAL;
using GameBrain;
using MenySystem;

namespace ConsoleApp;

public class GameController
{
    private static readonly ConfigRepository ConfigRepository = new ConfigRepository();

    private static (string, string) GetUsersNames()
    {
        string player1, player2;
        Console.Clear();
        Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m");
        Console.WriteLine("\x1b[1m\x1b[36m====================\x1b[0m\x1b[0m");
        Console.Write("Enter 1st Player's name: ");
        player1 = Console.ReadLine() ?? "Player 1";

        do
        {
            Console.Write("Enter 2nd Player's name: ");
            player2 = Console.ReadLine() ?? "Player 2";

            if (player2 != player1)
            {
                break;
            }
            
            Console.WriteLine("The 2nd player's name mus be different from the 1st player's name. Please enter different name.");
        } while (true);
        
        return (player1, player2);
    }
    
    private static EGamePiece GetStartingPlayer(string playerX, string playerO) 
    {
            Console.Clear();
            Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m");
            Console.WriteLine("\x1b[1m\x1b[36m====================\x1b[0m\x1b[0m");
            Console.WriteLine("Who will start the game?");
            Console.WriteLine($"    1. {playerX}");
            Console.WriteLine($"    2. {playerO}");
            do
            {
                Console.Write("Insert 1 or 2 to choose who starts: ");
                var startChoice = Console.ReadLine()?.Trim();
            
                if (startChoice == "1") return EGamePiece.X;
                if (startChoice == "2") return EGamePiece.O;

                Console.WriteLine("Invalid choice! Please enter '1' for the first player or '2' for the second player.");

            } while (true);
    }

    private static (string, string) GetPlayersSymbol(string playerX, string playerO)
    {
        string symbolX, symbolO;

        do
        {
            Console.Write($"Enter the symbol for {playerX} (default is 'X'):");
            symbolX = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(symbolX))
            {
                symbolX = "X";
            } 
            //else if (symbolX.Length > 1)
            // {
            //     Console.WriteLine("Please enter only a single character.");
            //     symbolX = null;
            // }
            
        } while (symbolX == null);

        do
        {
            Console.Write($"Enter the symbol for {playerO} (default is 'O'):");
            symbolO = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(symbolO))
            {
                symbolO = "O";
            }
            else if (symbolO == symbolX)
            {
                Console.WriteLine("The symbols for each player must be different.");
                symbolO = null;
            }
            // else if (symbolO.Length > 1)
            // {
            //     Console.WriteLine("Please enter only a single character.");
            //     symbolO = null;
            // }
        } while (symbolO == null);

        return (symbolX, symbolO);
    }
    
    public static string MainLoop()
    {
        int chosenConfigIndex = ChooseConfiguration();
        var configNames = ConfigRepository.GetConfigurationNames();

        GameConfiguration chosenConfig;
        
        if (chosenConfigIndex == configNames.Count)
        {
            chosenConfig = ConfigRepository.ConfigureCustomGame();
        }
        else
        {
            chosenConfig = ConfigRepository.GetConfigurationByName(configNames[chosenConfigIndex]);
        }
        
        var (playerX, playerO) = GetUsersNames();

        var (symbolX, symbolO) = GetPlayersSymbol(playerX, playerO);
        Visualizer.SetPlayersSymbols(symbolX, symbolO);
        
        EGamePiece startingPlayer = GetStartingPlayer(playerX, playerO);

        var gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO, startingPlayer);
        var moves = 0;
        
        do
        {
            Console.Clear();
            Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m (🥷 🆚 🤺)");
            Console.WriteLine("\x1b[1m\x1b[36m" + new string('=', gameInstance.DimX * 4) + "\x1b[0m\x1b[0m");
            Console.WriteLine();
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Console.WriteLine();
            
            if (moves >= 4)
            {
                Console.WriteLine("Choose what you want to do:");
                Console.WriteLine("\x1b[1m\x1b[35mA)\x1b[0m\x1b[0m Make a move");
                Console.WriteLine("\x1b[1m\x1b[35mB)\x1b[0m\x1b[0m Move the grid");
                Console.WriteLine("\x1b[1m\x1b[35mC)\x1b[0m\x1b[0m Move the piece");
           
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
                                Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m");
                                Console.WriteLine("\x1b[1m\x1b[36m" + new string('=', gameInstance.DimX * 4) + "\x1b[0m\x1b[0m");
                                Console.WriteLine();
                                Console.WriteLine($"{gameInstance.GetCurrentPlayer()} has won the game! \ud83e\udd73\ud83e\udd73\ud83e\udd73\ud83e\udd73 ");
                                Console.WriteLine();
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
    }

    private static int ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();
        var configNames = ConfigRepository.GetConfigurationNames();

        for (var i = 0; i < configNames.Count; i++)
        {
            var indexReturnValue = i;
            configMenuItems.Add(new MenuItem()
            {
                Title = configNames[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => indexReturnValue.ToString()
            });
        }

        var customConfigIndex = configNames.Count();
        configMenuItems.Add(new MenuItem()
        {
            Title = "Custom Configuration",
            Shortcut = (configMenuItems.Count + 1).ToString(),
            MenuItemAction = () =>customConfigIndex.ToString()
        });
        
        var configMenu = new Menu(EMenuLevel.Secondary,
            "\x1b[1m\x1b[35mTIC-TAC-TWO - choose configuration\x1b[0m\x1b[0m",
            configMenuItems,
            isCustomMenu: true
        );

        return int.Parse(configMenu.Run());
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
        Console.WriteLine($"{gameInstance.GetCurrentPlayer()}'s turn to make a move");
        Console.Write("Give me coordinates <\x1b[1m\x1b[31mX\x1b[0m\x1b[0m,\x1b[1m\x1b[32mY\x1b[0m\x1b[0m> or save:");
    
       
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