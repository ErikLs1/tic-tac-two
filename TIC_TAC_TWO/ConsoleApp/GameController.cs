using ConsoleUI;
using DAL;
using GameBrain;
using MenySystem;

namespace ConsoleApp;

public class GameController
{
    public static string MainLoop(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        GameHelpers.InitializeRepositories(configRepository, gameRepository);
        
        var chosenConfig = GameHelpers.SelectConfiguration();
        EGamePiece startingPlayer = GameHelpers.GetStartingPlayer();
        var gameInstance = new TicTacTwoBrain(chosenConfig, startingPlayer);

        while (true)
        {
            Console.Clear();
            GameHelpers.DisplayHeader("BATTLE (🥷 🆚 🤺)");
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Console.WriteLine();

            if (gameInstance.MoveCount >= 4)
            {
                string choice = GameHelpers.GetPlayerChoice();
                GameHelpers.HandlePlayerChoice(choice, gameInstance);
            }
            else
            {
                GameHelpers.ExecuteMove(gameInstance);
            }
        }
    }
    
    public static void MainLoopWithLoadedGame(GameState gameState, IConfigRepository configRepository, IGameRepository gameRepository)
    {
        GameHelpers.InitializeRepositories(configRepository, gameRepository);

        var gameInstance = new TicTacTwoBrain(gameState);
        
        while (true)
        {
            Console.Clear();
            GameHelpers.DisplayHeader("TIC-TAC-TWO (🥷 🆚 🤺)");
            Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Console.WriteLine();

            if (gameInstance.MoveCount >= 4)
            {
                string choice = GameHelpers.GetPlayerChoice();
                GameHelpers.HandlePlayerChoice(choice, gameInstance);
            }
            else
            {
                GameHelpers.ExecuteMove(gameInstance);
            }
        }
    }
}