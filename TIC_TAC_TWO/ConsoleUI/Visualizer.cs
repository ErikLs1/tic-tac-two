using GameBrain;

namespace ConsoleUI;

public class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance, int gridX, int gridY)
    {
        for (var row = 0; row < gameInstance.DimY; row++)
        {
            for (var col = 0; col < gameInstance.DimX; col++)
            {
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[col, row]) + " ");
                if (col == gameInstance.DimX - 1)
                {
                    continue;
                }
                else if (1 <= row + gridY && row + gridY <= 3 && 1 <= col + gridX && col + gridX <= 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("|");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("|");
                }
            }
            
            Console.WriteLine();
            
            if (row < gameInstance.DimX - 1)
            {
                for (var col = 0; col < gameInstance.DimX; col++)
                {
                    if (1 <= row + gridY && row + gridY < 3 && 1 <= col + gridX && col + gridX <= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("---");
                        Console.ResetColor();
                        if (col < gameInstance.DimX - 1 && col + gridX < gameInstance.DimX - 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("+");
                            Console.ResetColor();
                        }
                        else if (col != 4)
                        {
                            Console.Write("+");
                        }
                    }
                    else
                    {
                        Console.Write("---");
                        if (col < 4) Console.Write("+");
                    }
                }
            }
            Console.WriteLine();
        }
    }

    private static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}
