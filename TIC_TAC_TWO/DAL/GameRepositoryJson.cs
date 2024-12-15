using System.Text.Json;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    private static readonly string SaveDirectory = Path.Combine(FileHelper.BasePath, "Saves");
    private const string GameExtension = ".game.json";
    private readonly IConfigRepository _configRepo;

    public GameRepositoryJson()
    {
        _configRepo = new ConfigRepositoryJson();
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
    }

    public void SaveGame(GameState gameState, string gameConfigName)
    {
        if (gameState == null)
        {
            throw new Exception();
        }

        var existingConfigs = _configRepo.GetConfigurationNames();
        var config =
            existingConfigs.FirstOrDefault(c => c.Name.Equals(gameConfigName, StringComparison.OrdinalIgnoreCase));
        if (config.Id == 0)
        {
            throw new Exception($"Configuration '{gameConfigName}' does not exist.");
        }

        gameState.GameConfig.ConfigId = config.Id;
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
            fileName = $"{gameConfigName}_ID{gameState.GameId.Value}{GameExtension}";
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

        var files = Directory.GetFiles(SaveDirectory,$"*{GameExtension}");

        foreach (var file in files)
        {
            try
            {
                var json = File.ReadAllText(file);
                var gameState = JsonSerializer.Deserialize<GameState>(json);
                if (gameState != null && gameState.GameId.HasValue)
                {
                    savedGames.Add(gameState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse file {file}: {ex.Message}");
            }
        }

        return savedGames;
    }

    public GameState LoadGame(int gameId) 
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

        var config = _configRepo.GetConfigurationById(gameState.GameConfig.ConfigId);
        gameState.GameConfig = config;
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
    }

    private int GetNextGameId()
    {
        var savedGames = GetSavedGames();
        return savedGames.Any() ? savedGames.Max(g => g.GameId!.Value) + 1 : 1;
    }

    private string? GetFileNameById(int gameId)
    {
        var files = Directory.GetFiles(SaveDirectory, $"*ID{gameId}*{GameExtension}");
        return files.FirstOrDefault() != null ? Path.GetFileName(files.First()) : null;
    }
}