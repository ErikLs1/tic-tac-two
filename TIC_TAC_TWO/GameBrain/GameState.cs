namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;

    public GameConfiguration GameConfiguration { get; set; }
    public string PlayerX { get; set; }
    public string PlayerO { get; set; }
    public int MoveCount { get; set; }
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}