using System.Text.Json;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    private static readonly string SaveDirectory = Path.Combine(FileHelper.BasePath, "Saves");
    public void SaveGame(GameState gameState, string gameConfigName)
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        string fileName;
        
        if (gameState.GameId.HasValue)
        {
            fileName = GetFileNameById(gameState.GameId.Value)!;
            if (fileName == null)
            {
                throw new FileNotFoundException($"No saved games found with GameId {gameState.GameId.Value}");
            }
        }
        else
        {
            gameState.GameId = GetNextGameId();
            fileName = $"{gameConfigName}_ID{gameState.GameId.Value}{FileHelper.GameExtension}";
        }

        var filePath = Path.Combine(SaveDirectory, fileName);
        var json = JsonSerializer.Serialize(gameState);
        File.WriteAllText(filePath, json);
        Console.WriteLine($"Game saved successfully at {filePath}");
    }

    public List<GameState> GetSavedGames()
    {
        var savedGames = new List<GameState>();
        
        if (!Directory.Exists(SaveDirectory))
        {
            return savedGames;
        }

        var files = Directory.GetFiles(SaveDirectory,$"*{FileHelper.GameExtension}");

        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            var gameState = JsonSerializer.Deserialize<GameState>(json);
            if (gameState != null)
            {
                savedGames.Add(gameState);
            }
        }

        return savedGames;
    }

    public GameState LoadGame(int gameId) //game id
    {
        var fileName = GetFileNameById(gameId);
        if (fileName == null)
        {
            throw new FileLoadException($"Game with ID {gameId} not found");
        }

        var filePath = Path.Combine(SaveDirectory, fileName);
        var json = File.ReadAllText(filePath);
        var gameState = JsonSerializer.Deserialize<GameState>(json);
        
        if (gameState == null)
        {
            throw new FileLoadException("Game not found");
        }

        
        return gameState;
    }
    
    public void DeleteGame(int gameId)
    {
        var fileName = GetFileNameById(gameId);
        
        if (fileName != null)
        {
            var filePath = Path.Combine(SaveDirectory, fileName);
            File.Delete(filePath);
            Console.WriteLine($"Game with ID {gameId} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Game with ID {gameId} not found.");
        }
        
        //var filePath = Path.Combine(FileHelper.BasePath, Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar + gameName + FileHelper.GameExtension);
    }

    private int GetNextGameId()
    {
        var savedGames = GetSavedGames();
        return savedGames.Any() ? savedGames.Max(g => g.GameId!.Value) + 1 : 1;
    }

    private string? GetFileNameById(int gameId)
    {
        var files = Directory.GetFiles(SaveDirectory, $"*ID{gameId}*{FileHelper.GameExtension}");
        return files.FirstOrDefault() != null ? Path.GetFileName(files.First()) : null;
    }
}