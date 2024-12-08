using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Game
{
    // Primary Key
    public int Id { get; set; }
    public GameConfiguration Configuration { get; set; } = default!;
    
    // Foreign key

    public int PlayerXId { get; set; }
    public User PlayerX { get; set; } = default!;
    
    public int PlayerOId { get; set; }
    public User PlayerO { get; set; } = default!;
    
    //======================================
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(128)]
    public string SaveName { get; set; } = default!;
    
    public int NextMoveBy { get; set; }
    public int MoveCount { get; set; }
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    [MaxLength(10240)]
    public string GameBoardSerialized { get; set; } = default!;

    [MaxLength(128)]
    public string PlayerXSymbol { get; set; } = "X";
    
    [MaxLength(128)]
    public string PlayerOSymbol { get; set; } = "O";
}