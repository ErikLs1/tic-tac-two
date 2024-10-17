using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance, int gridX, int gridY)
    {
    for (var row = 0; row < gameInstance.DimY; row++)
    {
        for (var col = 0; col < gameInstance.DimX; col++)
        {
            bool isInGridArea = row >= gridY && row < gridY + gameInstance.GridHeight
                                             && col >= gridX && col < gridX + gameInstance.GridWidth;

            var piece = gameInstance.GetPiece(col, row);
                
            if (piece == EGamePiece.X)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (piece == EGamePiece.O)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ResetColor();
            }
                
            Console.Write(" " + DrawGamePiece(piece) + " ");
            Console.ResetColor();
                
            if (col < gameInstance.DimX - 1)
            {
                if (isInGridArea && col < gridX + gameInstance.GridWidth - 1)
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
        }

        Console.WriteLine();

        if (row < gameInstance.DimY - 1)
        {
            for (var col = 0; col < gameInstance.DimX; col++)
            {
                bool isInGridArea = row >= gridY && row < gridY + gameInstance.GridHeight - 1
                                                 && col >= gridX && col < gridX + gameInstance.GridWidth;

                if (isInGridArea)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("---");
                    Console.ResetColor();
                        
                    if (col < gameInstance.DimX - 1 && col < gridX + gameInstance.GridWidth - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("+");
                        Console.ResetColor();
                    }
                    else if (col < gameInstance.DimX - 1)
                    {
                        Console.Write("+");
                    }
                }
                else
                {
                    Console.Write("---");
                    if (col < gameInstance.DimX - 1) Console.Write("+");
                }
            }
            Console.WriteLine();
        }
    }
        
    Console.ResetColor();
    }


    private static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}