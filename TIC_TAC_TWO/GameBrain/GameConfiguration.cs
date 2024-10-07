namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;

    // how many pieces in straight to win the game
    public int WinCondition { get; set; } = 3;

    // 0 disabled
    public int MovePieceAfterNMoves { get; set; } = 2;

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, to win: {WinCondition}, can move piece after {MovePieceAfterNMoves} moves";

}