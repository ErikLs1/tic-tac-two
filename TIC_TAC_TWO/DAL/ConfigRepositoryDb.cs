using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        throw new NotImplementedException();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        throw new NotImplementedException();
    }

    public void SaveConfiguration(GameConfiguration gameConfig)
    {
        throw new NotImplementedException();
    }
}