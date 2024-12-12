namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    
    public string? SaveName { get; set; }
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    public int GridWidth { get; set; } = 3;
    public int GridHeight { get; set; } = 3;
    public int WinCondition { get; set; } = 3;
    public int MovePieceAfterNMoves { get; set; } = 4;

    public override string ToString() =>
        $"Board size is {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"Grid size is {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win you need to put {WinCondition} pieces in the grid straight or diagonal, " +
        $"You can move your piece after {MovePieceAfterNMoves} moves";
}