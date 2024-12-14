using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<(int Id, string Name)> GetConfigurationNames(); // tuple (id and name) GetConfigurationNames(id, name)
    GameConfig GetConfigurationById(int id); // ID

    int SaveConfigurationAndGetId(GameConfig gameConfig); // get id to pass it
    void SaveConfiguration(GameConfig gameConfig);

}