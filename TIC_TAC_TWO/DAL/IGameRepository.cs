using GameBrain;

namespace DAL;

public interface IGameRepository
{
    void SaveGame(GameState gameState, string gameConfigName);
    GameState LoadGame(string gameConfigName);
    List<string> GetSavedGames();
    
}