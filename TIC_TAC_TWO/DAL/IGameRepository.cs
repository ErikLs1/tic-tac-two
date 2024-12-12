using GameBrain;

namespace DAL;

public interface IGameRepository
{
    void SaveGame(GameState gameState, string gameConfigName);  // update in the function save by id
    GameState LoadGame(int gameId); //Id
    List<GameState> GetSavedGames();
    void DeleteGame(int gameId);
}