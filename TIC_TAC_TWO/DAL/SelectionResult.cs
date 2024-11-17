using GameBrain;

namespace DAL;

public class SelectionResult
{
    public bool IsNewGame { get; set; }
    public GameConfiguration Configuration { get; set; }
    public GameState SavedGame { get; set; }
}