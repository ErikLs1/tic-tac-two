using System.Text.Json;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    private static readonly string SaveDirectory = Path.Combine(FileHelper.BasePath, "Saves");
    // "/Users/eriklihhats/Desktop/Dev/icd0008-24f/Configurations/";
    public void SaveGame(GameState gameState, string gameConfigName)
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        
        var fileName = $"{gameConfigName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.game.json";
        var filePath = Path.Combine(SaveDirectory, fileName);
        var json = JsonSerializer.Serialize(gameState);
        File.WriteAllText(filePath, json);
        Console.WriteLine($"Game saved successfully at {filePath}");
    }

    public List<string> GetSavedGames()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            return new List<string>();
        }

        return Directory.GetFiles(SaveDirectory, "*.game.json")
            .Select(fullFileName => Path.GetFileNameWithoutExtension(fullFileName))
            .ToList();
    }
    
    public GameState LoadGame(string gameConfigName)
    {
        var fileName = $"{gameConfigName}.game.json";
        var filePath = Path.Combine(SaveDirectory, fileName);
        if (!File.Exists(filePath)) throw new FileLoadException("Game not found");

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<GameState>(json);
    }
}