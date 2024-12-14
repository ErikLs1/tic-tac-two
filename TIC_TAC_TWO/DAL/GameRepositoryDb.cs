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

        // Log the GameId before attempting to find the existing game
        Console.WriteLine($"Attempting to save GameState with GameId: {gameState.GameId}");

        Game? existingGame = null;

        if (gameState.GameId.HasValue)
        {
            existingGame = context.Games.Include(g => g.Configuration)
                .FirstOrDefault(g => g.Id == gameState.GameId.Value);

            Console.WriteLine(existingGame != null
                ? $"Found existing game with ID {gameState.GameId.Value}"
                : $"No existing game found with ID {gameState.GameId.Value}");
        }

        if (existingGame != null)
        {
            existingGame.SaveName = gameConfigName;
            existingGame.NextMoveBy = (int)gameState.NextMoveBy;
            existingGame.MoveCount = gameState.MoveCount;
            existingGame.GridPositionX = gameState.GridPositionX;
            existingGame.GridPositionY = gameState.GridPositionY;
            existingGame.GameBoardSerialized = JsonSerializer.Serialize(gameState.GameBoard);

            context.Games.Update(existingGame);
            Console.WriteLine($"Updated existing game with ID {existingGame.Id}");
        }
        else
        {
            Console.WriteLine("Creating a new game record.");

            var config = context.Configurations.FirstOrDefault(c => c.Name == gameConfigName);
            if (config == null)
            {
                config = new GameConfiguration()
                {
                    Name = gameState.GameConfig.Name,
                    BoardSizeWidth = gameState.GameConfig.BoardSizeWidth,
                    BoardSizeHeight = gameState.GameConfig.BoardSizeHeight,
                    GridWidth = gameState.GameConfig.GridWidth,
                    GridHeight = gameState.GameConfig.GridHeight,
                    WinCondition = gameState.GameConfig.WinCondition,
                    MovePieceAfterNMoves = gameState.GameConfig.MovePieceAfterNMoves
                };
                context.Configurations.Add(config);
                context.SaveChanges();
                Console.WriteLine($"Created new GameConfiguration with Name: {gameConfigName}");
            }

            var newGame = new Game()
            {
                CreatedAt = DateTime.Now,
                ConfigurationId = config.Id,
                SaveName = gameConfigName,
                NextMoveBy = (int)gameState.NextMoveBy,
                MoveCount = gameState.MoveCount,
                GridPositionX = gameState.GridPositionX,
                GridPositionY = gameState.GridPositionY,
                GameBoardSerialized = JsonSerializer.Serialize(gameState.GameBoard)
            };

            context.Games.Add(newGame);
            context.SaveChanges();

            gameState.GameId = newGame.Id;
            Console.WriteLine($"Created new game with ID {newGame.Id}");
        }

        context.SaveChanges();

        Console.WriteLine($"Game saved successfully with GameId {gameState.GameId}");
    }

    public GameState LoadGame(int gameId)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var game = context.Games
            .Include(g => g.Configuration)
            .FirstOrDefault(g => g.Id == gameId);

        if (game == null)
        {
            throw new Exception("Game not found");
        }

        var gameBoard = JsonSerializer.Deserialize<EGamePiece[][]>(game.GameBoardSerialized)!;

        var gameConfiguration = new GameBrain.GameConfig
        {
            Name = game.SaveName, // Change if needed
            BoardSizeWidth = game.Configuration.BoardSizeWidth,
            BoardSizeHeight = game.Configuration.BoardSizeHeight,
            GridWidth = game.Configuration.GridWidth,
            GridHeight = game.Configuration.GridHeight,
            WinCondition = game.Configuration.WinCondition,
            MovePieceAfterNMoves = game.Configuration.MovePieceAfterNMoves
        };

        var gameState = new GameState(gameBoard, gameConfiguration)
        {
            GameId = game.Id,
            NextMoveBy = (EGamePiece)game.NextMoveBy,
            MoveCount = game.MoveCount,
            GridPositionX = game.GridPositionX,
            GridPositionY = game.GridPositionY,
        };


        return gameState;
    }

    public List<GameState> GetSavedGames()
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var games = context.Games.Include(g => g.Configuration).ToList();

        var gameStates = games.Select(game =>
        {
            var gameBoard = JsonSerializer.Deserialize<EGamePiece[][]>(game.GameBoardSerialized)
                            ?? throw new Exception("Failed to deserialize game board");

            var gameConfiguration = new GameBrain.GameConfig
            {
                Name = game.SaveName,
                BoardSizeWidth = game.Configuration.BoardSizeWidth,
                BoardSizeHeight = game.Configuration.BoardSizeHeight,
                GridWidth = game.Configuration.GridWidth,
                GridHeight = game.Configuration.GridHeight,
                WinCondition = game.Configuration.WinCondition,
                MovePieceAfterNMoves = game.Configuration.MovePieceAfterNMoves
            };

            return new GameState(gameBoard, gameConfiguration)
            {
                GameId = game.Id,
                NextMoveBy = (EGamePiece)game.NextMoveBy,
                MoveCount = game.MoveCount,
                GridPositionX = game.GridPositionX,
                GridPositionY = game.GridPositionY
            };
        }).OrderByDescending(gs => gs.GameId).ToList();

        return gameStates;
    }

    public void DeleteGame(int gameId)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var game = context.Games.FirstOrDefault(g => g.Id == gameId);
        if (game != null)
        {
            context.Games.Remove(game);
            context.SaveChanges();
            Console.WriteLine($"Game with ID {gameId} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Game with ID {gameId} not found.");
        }
    }
}