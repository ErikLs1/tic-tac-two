using System.ComponentModel.DataAnnotations;

namespace Domain;

public class GameConfiguration
{
    // Primary Key
    public int Id { get; set; }

    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; }
    public int BoardSizeHeight { get; set; }
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public int WinCondition { get; set; }
    public int MovePieceAfterNMoves { get; set; }
    
    //public ICollection<Game>? Games { get; set; }
}