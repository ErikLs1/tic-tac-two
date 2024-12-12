using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<(int Id, string Name)> GetConfigurationNames(); // tuple (id and name) GetConfigurationNames(id, name)
    GameConfiguration GetConfigurationById(int id); // ID

    void SaveConfiguration(GameConfiguration gameConfig);
}