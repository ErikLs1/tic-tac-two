namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    public EGamePiece _nextMoveBy { get; set; } = EGamePiece.X;

    private GameConfiguration _gameConfiguration;

    private string _playerX;
    private string _playerO;
    
    private (int x, int y) _gridPostion;


    public TicTacTwoBrain(GameConfiguration gameConfiguration, string playerX, string playerO, EGamePiece startingPlayer)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        
        _playerX = playerX;
        _playerO = playerO;
        _nextMoveBy = startingPlayer;
        
        for (int x = 0; x < _gameConfiguration.BoardSizeWidth; x++)
        {
            for (int y = 0; y < _gameConfiguration.BoardSizeHeight; y++)
            {
                _gameBoard[x, y] = EGamePiece.Empty;
            }
        }
        _gridPostion = GetGridCenter();
    }

    public bool CanMoveGrid(int directionX, int directionY)
    {
        int targetX = _gridPostion.x + directionX;
        int targetY = _gridPostion.y + directionY;

        if (targetX >= 0 && targetX <= DimX - GridWidth && targetY >= 0 && targetY <= DimY - GridHeight)
        {
            _gridPostion = (targetX, targetY);
            return true;
        }
        
        Console.WriteLine("Cannot move grid in this direction. Choose the other move");
        return false;
    }

    public string GetCurrentPlayer()
    {
        return _nextMoveBy == EGamePiece.X ? _playerX : _playerO;
    }
    private (int x, int y) GetGridCenter()
    {
        int startX = (DimX - GridWidth) / 2;
        int startY = (DimY - GridHeight) / 2;

        return (startX, startY);
    }
    
    public int DimX => _gameBoard.GetLength(0);
    public int DimY => _gameBoard.GetLength(1);

    public (int x, int y) GridPosition => _gridPostion;
    public int GridWidth => _gameConfiguration.GridWidth;
    public int GridHeight => _gameConfiguration.GridHeight;
    public EGamePiece GetPiece(int x, int y) => _gameBoard[x, y];

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x, y] = _nextMoveBy;
        
        if (CheckForWin(x, y) != null)
        {
            return true;
        }
        
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        return true;
    }
    
    public void MoveAPiece(int startX, int startY, int targetX, int targetY)
    {
        if (_gameBoard[startX, startY] == _nextMoveBy)
        {
            _gameBoard[startX,startY] = EGamePiece.Empty;
            
            _gameBoard[targetX, targetY] = _nextMoveBy;
            
            if (CheckForWin(targetX, targetY) != null)
            {
                Console.WriteLine($"{_nextMoveBy} wins!");
                return;
            }
            
            _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        else
        {
            Console.WriteLine("Invalid move. The start position does not contain your piece.");
        }
    }
    
    public EGamePiece? CheckForWin(int x, int y)
    {
        if (CheckDirection(x, y, 1, 0, _nextMoveBy) ||
            CheckDirection(x, y, 0, 1, _nextMoveBy) ||
            CheckDirection(x, y, 1, 1, _nextMoveBy) ||
            CheckDirection(x, y, 1, -1, _nextMoveBy))
        {
            return _nextMoveBy;
        }
        return null;
    }

    private bool CheckDirection(int x, int y, int deltaX, int deltaY, EGamePiece player)
    {
        int count = 1;
        count += CountInDirection(x, y, deltaX, deltaY, player);
        count += CountInDirection(x, y, -deltaX, -deltaY, player);

        return count >= _gameConfiguration.WinCondition;
    }

    private int CountInDirection(int x, int y, int deltaX, int deltaY, EGamePiece player)
    {
        int count = 0;

        for (int i = 1; i < _gameConfiguration.WinCondition; i++)
        {
            int checkX = x + i * deltaX;
            int checkY = y + i * deltaY;
            
            if (checkX >= 0 && checkX < _gameConfiguration.BoardSizeWidth &&
                checkY >= 0 && checkY < _gameConfiguration.BoardSizeHeight &&
                _gameBoard[checkX, checkY] == player)
            {
                count++;
            }
            else
            {
                break;
            }
        }

        return count;
    }

    // public EGamePiece? CheckForWin(int inputX, int inputY)
    // {
    //     EGamePiece player = _gameBoard[inputX, inputY];
    //     
    //     if (CheckHorizontal(inputX, inputY, player) ||
    //         CheckVertical(inputX, inputY, player))
    //     {
    //         return player;
    //     }
    //     return null;
    // }
    //
    // private bool CheckHorizontal(int inputX, int inputY, EGamePiece currentPiece)
    // {
    //     return CheckInDirection(inputX, inputY, 1, 0, currentPiece) + 
    //         CheckInDirection(inputX, inputY, -1, 0, currentPiece) >= _gameConfiguration.WinCondition - 1; 
    // }
    //
    // private bool CheckVertical(int inputX, int inputY, EGamePiece currentPiece)
    // {
    //     return CheckInDirection(inputX, inputY, 0, 1, currentPiece) +
    //         CheckInDirection(inputX, inputY, 0, -1, currentPiece) >= _gameConfiguration.WinCondition - 1;
    // }
    //
    // private bool CheckDiagonal(int inputX, int inputY, EGamePiece currentPiece)
    // {
    //     return CheckInDirection(inputX, inputY, 1, 1, currentPiece) +
    //         CheckInDirection(inputX, inputY, -1, -1, currentPiece) + 
    //         CheckInDirection(inputX, inputY, -1, 1, currentPiece) +
    //         CheckInDirection(inputX, inputY, 1, -1, currentPiece) >= _gameConfiguration.WinCondition - 1; 
    // }
    //
    //
    //
    // private int CheckInDirection(int startX, int startY, int x, int y, EGamePiece player)
    // {
    //     int count = 0;
    //
    //     for (int i = 1; i < _gameConfiguration.WinCondition; i++)
    //     {
    //         int checkX = startX + i * x;
    //         int checkY = startY + i * y;
    //         
    //         if (checkX >= 0 && checkX < _gameConfiguration.BoardSizeWidth &&
    //             checkY >= 0 && checkY < _gameConfiguration.BoardSizeHeight &&
    //             _gameBoard[checkX, checkY] == player)
    //         {
    //             count++;
    //         }
    //         else
    //         {
    //             break;
    //         }
    //     }
    //
    //     return count;
    // }
    
    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        _nextMoveBy = EGamePiece.X;
    }
}