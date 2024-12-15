using System.Text.Json;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    private static readonly string ConfigDirectory = Path.Combine(FileHelper.BasePath, "Configs");
    private const string ConfigExtension = ".config.json";

    public ConfigRepositoryJson()
    {
        CheckAndCreateInitialConfig();
    }

    public List<(int Id, string Name)> GetConfigurationNames()
    {
        var configFiles = Directory
            .GetFiles(ConfigDirectory, "*" + ConfigExtension)
            .OrderBy(f => f)
            .ToList();

        var configurations = configFiles
            .Select(file =>
            {
                var config = JsonSerializer.Deserialize<GameConfig>(File.ReadAllText(file));
                return config != null 
                    ? (Id: config.ConfigId, Name: config.Name) 
                    : (Id: 0, Name: "Invalid Config");
            })
            .Where(c => c.Id != 0)
            .ToList();

        return configurations;
    }


    public GameConfig GetConfigurationById(int id)
    {
        var configFile = Directory
            .GetFiles(ConfigDirectory, "*" + ConfigExtension)
            .FirstOrDefault(f =>
            {
                try
                {
                    var config = JsonSerializer.Deserialize<GameConfig>(File.ReadAllText(f));
                    return config != null && config.ConfigId == id;
                }
                catch
                {
                    return false;
                }
            });

        if (configFile == null)
        {
            throw new Exception($"Configuration with ID {id} does not exist.");
        }

        var configJsonStr = File.ReadAllText(configFile);
        var gameConfig = JsonSerializer.Deserialize<GameConfig>(configJsonStr);

        if (gameConfig == null)
        {
            throw new Exception($"Failed to deserialize configuration from file: {configFile}");
        }

        return gameConfig;
    }

    public void SaveConfiguration(GameConfig gameConfig)
    {
        if (gameConfig == null)
        {
            throw new ArgumentNullException(nameof(gameConfig));
        }

        if (gameConfig.ConfigId > 0)
        {
            // Existing configuration, update it
            var existingFile = Directory
                .GetFiles(ConfigDirectory, "*" + ConfigExtension)
                .FirstOrDefault(f =>
                {
                    try
                    {
                        var config = JsonSerializer.Deserialize<GameConfig>(File.ReadAllText(f));
                        return config != null && config.ConfigId == gameConfig.ConfigId;
                    }
                    catch
                    {
                        return false;
                    }
                });

            if (existingFile != null)
            {
                // Overwrite the existing file
                var updatedJson =
                    JsonSerializer.Serialize(gameConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(existingFile, updatedJson);
                Console.WriteLine($"Configuration with ID {gameConfig.ConfigId} updated successfully.");
                return;
            }
            else
            {
                throw new Exception(
                    $"Cannot update configuration. Configuration with ID {gameConfig.ConfigId} does not exist.");
            }
        }
        else
        {
            // New configuration, assign a new ConfigId
            gameConfig.ConfigId = GetNextConfigId();

            // Construct a filename that includes the ConfigId and Name
            string safeName = MakeSafeFilename(gameConfig.Name);
            string newFileName = $"{gameConfig.ConfigId}_{safeName}{ConfigExtension}";
            string newFilePath = Path.Combine(ConfigDirectory, newFileName);

            var newConfigJson =
                JsonSerializer.Serialize(gameConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(newFilePath, newConfigJson);
            Console.WriteLine(
                $"New configuration '{gameConfig.Name}' saved successfully with ID {gameConfig.ConfigId}.");
        }
    }


    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
        }

        var configFiles = Directory.GetFiles(ConfigDirectory, "*" + ConfigExtension).ToList();

        if (configFiles.Count == 0)
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

    private int GetNextConfigId()
    {
        var configIds = Directory
            .GetFiles(ConfigDirectory, "*" + ConfigExtension)
            .Select(f =>
            {
                try
                {
                    var config = JsonSerializer.Deserialize<GameConfig>(File.ReadAllText(f));
                    return config != null ? config.ConfigId : 0;
                }
                catch
                {
                    return 0;
                }
            })
            .Where(id => id > 0)
            .ToList();

        return configIds.Any() ? configIds.Max() + 1 : 1;
    }

    private string MakeSafeFilename(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        return name;
    }
}