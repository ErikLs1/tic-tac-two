namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 3;
    public int BoardSizeHeight { get; set; } = 3;

    public int WinCondition { get; set; } = 3;

    public int CanMovePieceAfterNMoves { get; set; } = 0;

    public override string ToString() => 
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, to win: {WinCondition}, can move piece after {CanMovePieceAfterNMoves} moves";
}