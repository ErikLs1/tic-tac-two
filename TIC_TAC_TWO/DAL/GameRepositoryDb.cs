using GameBrain;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName)
    {
        throw new NotImplementedException();
    }

    public GameState LoadGame(string gameConfigName)
    {
        throw new NotImplementedException();
    }

    public List<string> GetSavedGames()
    {
        throw new NotImplementedException();
    }
}