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
        
        GameConfiguration chosenConfig = GameHelpers.SelectConfiguration();
        var (playerX, playerO) = GameHelpers.GetUsersNames();
        var (symbolX, symbolO) = GameHelpers.GetPlayersSymbol(playerX, playerO);
        Visualizer.SetPlayersSymbols(symbolX, symbolO);
        
        EGamePiece startingPlayer = GameHelpers.GetStartingPlayer(playerX, playerO);
        var gameInstance = new TicTacTwoBrain(chosenConfig, playerX, playerO, startingPlayer);
        var moves = 0;

        while (true)
        {
            Console.Clear();
            GameHelpers.DisplayHeader("TIC-TAC-TWO (🥷 🆚 🤺)");
            ConsoleUI.Visualizer.DrawBoard(gameInstance, gameInstance.GridPosition.x, gameInstance.GridPosition.y);
            Console.WriteLine();

            if (moves >= 4)
            {
                string choice = GameHelpers.GetPlayerChoice();
                GameHelpers.HandlePlayerChoice(choice, gameInstance, ref moves);
            }
            else
            {
                GameHelpers.ExecuteMove(gameInstance);
                moves++;
            }
        }
    }
}