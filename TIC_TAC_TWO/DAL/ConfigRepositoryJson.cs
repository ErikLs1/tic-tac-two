using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        return Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .Select(fullFileName =>
                Path.GetFileNameWithoutExtension(
                    Path.GetFileNameWithoutExtension(fullFileName)
                )
            )
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
        var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
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