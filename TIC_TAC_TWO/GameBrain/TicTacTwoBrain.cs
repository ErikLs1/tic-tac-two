using MenySystem;

namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    public EGamePiece _nextMoveBy { get; set; } = EGamePiece.X;

    private GameConfiguration _gameConfiguration;

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
    }

    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }
    // get board GameBoard
    public int DimX => _gameBoard.GetLength(0);
    public int DimY => _gameBoard.GetLength(1);

    private EGamePiece[,] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var x = 0; x < _gameBoard.GetLength(0); x++)
        {
            for (var y = 0; y < _gameBoard.GetLength(1); y++)
            {
                copyOfBoard[x, y] = _gameBoard[x, y];
            }            
        }

        return copyOfBoard;
    }

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x, y] = _nextMoveBy;

        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }
 
    public void MoveAPiece(int startX, int startY, int targetX, int targetY)
    {
        if (_gameBoard[startX, startY] == _nextMoveBy)
        {
            _gameBoard[startX,startY] = EGamePiece.Empty;
            
            _gameBoard[targetX, targetY] = _nextMoveBy;
            
            _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        else
        {
            Console.WriteLine("Invalid move. The start position does not contain your piece.");
        }
    }
    
    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        _nextMoveBy = EGamePiece.X;
    }
}