using System.Text.Json;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;
using GameConfiguration = Domain.GameConfiguration;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());

        var playerX = context.Users.FirstOrDefault(u => u.Username == gameState.PlayerX);
        if (playerX == null)
        {
            throw new Exception($"User '{gameState.PlayerX}' not found");
        }

        var playerO = context.Users.FirstOrDefault(u => u.Username == gameState.PlayerO);
        if (playerO == null)
        {
            throw new Exception($"User '{gameState.PlayerO}' not found");
        }

        // var config = context.Configurations.FirstOrDefault(c => c.Name == gameState.GameConfiguration.Name);


        var config = new GameConfiguration()
        {
            Name = gameState.GameConfiguration.Name,
            BoardSizeWidth = gameState.GameConfiguration.BoardSizeWidth,
            BoardSizeHeight = gameState.GameConfiguration.BoardSizeHeight,
            GridWidth = gameState.GameConfiguration.GridWidth,
            GridHeight = gameState.GameConfiguration.GridHeight,
            WinCondition = gameState.GameConfiguration.WinCondition,
            MovePieceAfterNMoves = gameState.GameConfiguration.MovePieceAfterNMoves
        };
        // context.Configurations.Add(config);
        // context.SaveChanges();
        
        var timeStamp = DateTime.Now;
        var saveNameWithTimestamp = $"{gameConfigName}_{timeStamp:yyyy-MM-dd_HH-mm-ss}";

        var game = new Game()
        {
            CreatedAt = timeStamp,
            SaveName = saveNameWithTimestamp,
            PlayerXId = playerX.Id,
            PlayerOId = playerO.Id,
            NextMoveBy = (int)gameState.NextMoveBy,
            MoveCount = gameState.MoveCount,
            GridPositionX = gameState.GridPositionX,
            GridPositionY = gameState.GridPositionY,
            GameBoardSerialized = JsonSerializer.Serialize(gameState.GameBoard),
            PlayerXSymbol = gameState.PlayerXSymbol,
            PlayerOSymbol = gameState.PlayerOSymbol,
            Configuration = config
        };

        context.Games.Add(game);
        context.SaveChanges();
    }

    public GameState LoadGame(string gameConfigName)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var game = context.Games
            .Include(g => g.Configuration)
            .Include(g => g.PlayerX)
            .Include(g => g.PlayerO)
            .FirstOrDefault(g => g.SaveName == gameConfigName);

        if (game == null)
        {
            throw new Exception("Game not found");
        }

        var gameBoard = JsonSerializer.Deserialize<EGamePiece[][]>(game.GameBoardSerialized)!;

        var gameConfiguration = new GameBrain.GameConfiguration
        {
            Name = game.Configuration.Name,
            BoardSizeWidth = game.Configuration.BoardSizeWidth,
            BoardSizeHeight = game.Configuration.BoardSizeHeight,
            GridWidth = game.Configuration.GridWidth,
            GridHeight = game.Configuration.GridHeight,
            WinCondition = game.Configuration.WinCondition,
            MovePieceAfterNMoves = game.Configuration.MovePieceAfterNMoves
        };

        var gameState = new GameState(gameBoard, gameConfiguration)
        {
            NextMoveBy = (EGamePiece)game.NextMoveBy,
            PlayerX = game.PlayerX.Username,
            PlayerO = game.PlayerO.Username,
            PlayerXSymbol = game.PlayerXSymbol,
            PlayerOSymbol = game.PlayerOSymbol,
            MoveCount = game.MoveCount,
            GridPositionX = game.GridPositionX,
            GridPositionY = game.GridPositionY,
        };


        return gameState;
    }

    public List<string> GetSavedGames()
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        return context.Games
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => g.SaveName)
            .ToList();
    }

    public void DeleteGame(string gameName)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var game = context.Games.FirstOrDefault(g => g.SaveName == gameName);
        if (game != null)
        {
            context.Games.Remove(game);
            context.SaveChanges();
        }
    }
}