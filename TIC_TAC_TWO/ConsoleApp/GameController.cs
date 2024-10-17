using DAL;
using GameBrain;
using MenySystem;

namespace ConsoleApp;

public static class GameController
{
     private static readonly ConfigRepository ConfigRepository = new ConfigRepository();
    private static int gridX = 0;
    private static int gridY = 0;

    public static string MainLoop()
    {
        var chosenConfigShortcut = ChooseConfiguration();

        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            return chosenConfigShortcut;
        }

        var chosenConfig = ConfigRepository.GetConfigurationByName(
            ConfigRepository.GetConfigurationNames()[configNo]
        );

        var gameInstance = new TicTacTwoBrain(chosenConfig);
        var moves = 0;
        
        do
        {
            
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gridX, gridY);

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
                        gameInstance.MakeAMove(inputX, inputY);
                        moves++;
                        Console.Clear();
                        break;

                    case "B":
                        Console.Write("Enter your move (up, down, left, right, up-left, up-right, down-left, down-right): ");
                        string move = Console.ReadLine()?.ToLower()!;
                        Console.WriteLine();
                        MoveGrid(move);
                        gameInstance. _nextMoveBy = gameInstance._nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                        Console.Clear();
                        break;

                    case "C":
                        MovePiece(gameInstance, gameInstance._nextMoveBy);
                        break;
                    // Implement winning condition
                    // Change board and grid settings
                    // Make correct menu
                    // Deal with errors (test the game)
                    // handle error that cannot put piece if place is occupied
                    
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

        return "Good Job PlayerName";
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

        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        return configMenu.Run();
    }
    
    static void MoveGrid(string direction)
    {
        if (direction == "up" && gridY < 1)
            gridY++;
        else if (direction == "down" && gridY > -1) 
            gridY--;
        else if (direction == "left" && gridX < 1)
            gridX++;
        else if (direction == "right" && gridX > -1) 
            gridX--;
        else if (direction == "up-left" && gridY > -1 && gridX < 1)
        {
            gridY++;
            gridX++;
        }
        else if (direction == "up-right" && gridY < 1 && gridX > -1)
        {
            gridY++;
            gridX--;
        }
        else if (direction == "down-left" && gridY > -1 && gridX < 1)
        {
            gridY--;
            gridX++;
        }
        else if (direction == "down-right" && gridY > -1 && gridX > -1)
        {
            gridY--;
            gridX--;
        }
    }

    private static (int, int) InsertCoordinates(TicTacTwoBrain gameInstance)
    {
        Console.Write("Give me coordinates <x,y> or save:");

        int inputX, inputY;
        while (true)
        {
            var input = Console.ReadLine()!;
            var inputSplit = input.Split(",");

            if (inputSplit.Length != 2)
            {
                Console.Write("Wrong input. Please make sure you input coordinates correctly <x,y>: ");
                continue;
            }

            if (!int.TryParse(inputSplit[0], out inputX) || !int.TryParse(inputSplit[1], out inputY))
            {
                Console.Write("Wrong coordinates. Please, make sure both x and y are numbers: ");
                continue;
            }

            if (gameInstance.GameBoard[inputX, inputY] != EGamePiece.Empty)
            {
                Console.Write("This spot is occupied. Please, choose different position <x,y>: ");
                continue;
            }
            return(inputX, inputY);
        }
    }

    private static void MovePiece(TicTacTwoBrain gameInstance, EGamePiece gamePiece)
    {
        int startX, startY, targetX, targetY;

        while (true)
        {
            Console.Write($"It's {gamePiece}'s turn. Enter the coordinates of the piece you want to move <x,y>: ");
            var startInput = Console.ReadLine()!;
            var startInputSplit = startInput.Split(",");

            if (startInputSplit.Length != 2 || !int.TryParse(startInputSplit[0], out startX) ||
                                                !int.TryParse(startInputSplit[1], out startY))
            {
                Console.WriteLine("Wrong input. PLease, make sure coordinates are in the correct format.");
                continue;
            }
            
            if (gameInstance.GameBoard[startX, startY] != gamePiece)
            {
                Console.WriteLine($"You can only move your own piece ({gamePiece}). Please, choose another piece.");
                continue;
            }
            
            Console.Write("Enter the coordinates where you want to move your piece <x,y>: ");
            var targetInput = Console.ReadLine()!;
            var targetInputSplit = targetInput.Split(",");

            if (targetInputSplit.Length != 2 || !int.TryParse(targetInputSplit[0], out targetX) ||
                !int.TryParse(targetInputSplit[1], out targetY))
            {
                Console.WriteLine("Wrong input. Please, make sure the coordinates are in the correct format. ");
                continue;
            }
            if (gameInstance.GameBoard[targetX, targetY] != EGamePiece.Empty)
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