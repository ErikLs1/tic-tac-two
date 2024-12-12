using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames(); // tuple (id and name) GetConfigurationNames(id, name)
    GameConfiguration GetConfigurationByName(string name); // ID

    void SaveConfiguration(GameConfiguration gameConfig);
}