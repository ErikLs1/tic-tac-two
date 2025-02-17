namespace GameBrain;

public class GameState
{
    public int? GameId { get; set; } = null;
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;

    public GameConfig GameConfig { get; set; }
    public int MoveCount { get; set; }
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }
    
    public GameState(EGamePiece[][] gameBoard, GameConfig gameConfig)
    {
        GameBoard = gameBoard;
        GameConfig = gameConfig;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}