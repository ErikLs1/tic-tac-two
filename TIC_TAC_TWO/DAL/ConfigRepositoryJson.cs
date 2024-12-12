using System.Text.Json;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    public List<(int Id, string Name)> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        var configFiles = Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .OrderBy(f => f) 
            .ToList();
        
        var configurationNames = configFiles
            .Select((fullFileName, index) => 
            (
                Id: index + 1, // Assign sequential IDs starting from 1
                Name: Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fullFileName))
            ))
            .ToList();
        
        return configurationNames;
    }

    public GameConfiguration GetConfigurationById(int id)
    {
        var configFiles = Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .OrderBy(f => f)
            .ToList();

        if (id < 1 || id > configFiles.Count)
        {
            throw new Exception($"Configuration with ID {id} does not exist.");
        }

        var selectedFile = configFiles[id - 1]; // IDs start at 1

        var configJsonStr = File.ReadAllText(selectedFile);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);

        if (config == null)
        {
            throw new Exception($"Failed to deserialize configuration from file: {selectedFile}");
        }

        return config;
    }

    public void SaveConfiguration(GameConfiguration gameConfig)
    {
        var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameConfig);
        System.IO.File.WriteAllText(FileHelper.BasePath + gameConfig.Name + FileHelper.ConfigExtension, optionJsonStr);
    }


    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }

        var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        
        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var configurationNames = ConfigRepositoryHardcoded.GetConfigurationNames();
            foreach (var name in configurationNames)
            {
                var gameConfig = ConfigRepositoryHardcoded.GetConfigurationByName(name);
                SaveConfiguration(gameConfig);
            }
        }
    }
}