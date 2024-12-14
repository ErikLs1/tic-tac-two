namespace GameBrain;

public class TicTacTwoBrain
{
    private readonly GameState _gameState;
    
    private EGamePiece[][] _gameBoard;
    public EGamePiece _nextMoveBy { get; set; } = EGamePiece.X;

    private GameConfig _gameConfig;
    
    
    private (int x, int y) _gridPosition;
    public int MovePieceAfterNMoves { get; set; }
    public int MoveCount { get; private set; }

    public TicTacTwoBrain (GameState gameState)
    {
        _gameState = gameState;
        _gameConfig = gameState.GameConfig;
        _gameBoard = gameState.GameBoard;
        _nextMoveBy = gameState.NextMoveBy;
        _gridPosition = (gameState.GridPositionX, gameState.GridPositionY);
        MoveCount = gameState.MoveCount;
        MovePieceAfterNMoves = _gameConfig.MovePieceAfterNMoves;
    }

    public TicTacTwoBrain(GameConfig gameConfig, EGamePiece startingPlayer)
    {
        _gameConfig = gameConfig;
        MovePieceAfterNMoves = gameConfig.MovePieceAfterNMoves;
        _nextMoveBy = startingPlayer;
        MoveCount = 0;

        _gameBoard = new EGamePiece[gameConfig.BoardSizeWidth][];
        for (var x = 0; x < _gameBoard.Length; x++)
        {
            _gameBoard[x] = new EGamePiece[gameConfig.BoardSizeHeight];
        }

        _gameState = new GameState(_gameBoard, gameConfig)
        {
            NextMoveBy = startingPlayer
        };

        _gridPosition = GetGridCenter();
    }
    
    public EGamePiece[][] GameBoard
    {
        get => GetBoard();
        private set => _gameState.GameBoard = value;
    }
    
    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard.GetLength(0)][];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {
                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }

        return copyOfBoard;
    }

    public GameState GetGameState()
    {
        return new GameState(GameBoard, _gameConfig)
        {
            GameId = _gameState.GameId,
            NextMoveBy = _nextMoveBy,
            MoveCount = MoveCount,
            GridPositionX = _gridPosition.x,
            GridPositionY = _gridPosition.y,
        };
    }
    public string GetGameStateJson()
    {
        return _gameState.ToString();
    }

    public string GetGameConfigName()
    {
        return _gameState.GameConfig.Name;
    }

    public bool CanMoveGrid(int directionX, int directionY)
    {
        int targetX = _gridPosition.x + directionX;
        int targetY = _gridPosition.y + directionY;

        if (targetX >= 0 && targetX <= DimX - GridWidth && targetY >= 0 && targetY <= DimY - GridHeight)
        {
            _gridPosition = (targetX, targetY);
            return true;
        }
        
        Console.WriteLine("Cannot move grid in this direction. Choose the other move");
        return false;
    }
    
    public static bool MoveGrid(TicTacTwoBrain gameInstance, string direction)
    {
        return direction switch
        {
            "up" => gameInstance.CanMoveGrid(0, -1),
            "down" => gameInstance.CanMoveGrid(0, 1),
            "left" => gameInstance.CanMoveGrid(-1, 0),
            "right" => gameInstance.CanMoveGrid(1, 0),
            "up-left" => gameInstance.CanMoveGrid(-1, -1),
            "up-right" => gameInstance.CanMoveGrid(1, -1),
            "down-left" => gameInstance.CanMoveGrid(-1, 1),
            "down-right" => gameInstance.CanMoveGrid(1, 1),
            _ => false
        };
    }
    
    public static bool IsWithinBounds(TicTacTwoBrain gameInstance, int x, int y)
    {
        int boardWidth = gameInstance.BoardWidth;
        int boardHeight = gameInstance.BoardHeight;

        return x >= 0 && x < boardWidth && y >= 0 && y < boardHeight;
    }
    public static bool IsWithinBoundsGrid(TicTacTwoBrain gameInstance, int x, int y)
    {
        var gridPosition = gameInstance.GridPosition;

        int gridLeft = gridPosition.x;
        int gridTop = gridPosition.y;
        int gridRight = gridLeft + gameInstance.GridWidth;
        int gridBottom = gridTop + gameInstance.GridHeight;
        return x >= gridLeft && x < gridRight && y >= gridTop && y < gridBottom;
    }

    public EGamePiece GetCurrentPlayer()
    {
        return _nextMoveBy == EGamePiece.X ? EGamePiece.X : EGamePiece.O;
    }
    private (int x, int y) GetGridCenter()
    {
        int startX = (DimX - GridWidth) / 2;
        int startY = (DimY - GridHeight) / 2;

        return (startX, startY);
    }
    
    public int DimX => _gameState.GameBoard.Length;
    public int DimY => _gameState.GameBoard[0].Length;

    public (int x, int y) GridPosition => _gridPosition;
    public int GridWidth => _gameConfig.GridWidth;
    public int GridHeight => _gameConfig.GridHeight;
    
    public int BoardWidth => _gameConfig.BoardSizeWidth;
    public int BoardHeight => _gameConfig.BoardSizeHeight;
    public EGamePiece GetPiece(int x, int y) => _gameBoard[x][y];

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x][y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x][y] = _nextMoveBy;
        
        if (CheckForWin(x, y) != null)
        {
            return true;
        }
        
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        MoveCount++;
        
        return true;
    }
    
    public void MoveAPiece(int startX, int startY, int targetX, int targetY)
    {
        if (_gameBoard[startX][startY] == _nextMoveBy)
        {
            _gameBoard[startX][startY] = EGamePiece.Empty;
            
            _gameBoard[targetX][targetY] = _nextMoveBy;
            
            if (CheckForWin(targetX, targetY) != null)
            {
                Console.WriteLine($"{_nextMoveBy} wins!");
                return;
            }
            
            _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

            MoveCount++;
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

        return count >= _gameConfig.WinCondition;
    }
    

    private int CountInDirection(int x, int y, int deltaX, int deltaY, EGamePiece player)
    {
        int count = 0;

        for (int i = 1; i < _gameConfig.WinCondition; i++)
        {
            int checkX = x + i * deltaX;
            int checkY = y + i * deltaY;
            
            if (checkX >= 0 && checkX < _gameConfig.BoardSizeWidth &&
                checkY >= 0 && checkY < _gameConfig.BoardSizeHeight &&
                _gameBoard[checkX][checkY] == player)
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
    
    public void ResetGame()
    {
        var gameBoard = new EGamePiece[_gameState.GameConfig.BoardSizeWidth][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[_gameState.GameConfig.BoardSizeWidth];
        }

        _gameState.GameBoard = gameBoard;
        _gameState.NextMoveBy = EGamePiece.X;
    }
    
    
}