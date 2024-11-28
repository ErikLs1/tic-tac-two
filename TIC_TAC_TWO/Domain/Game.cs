using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Game
{
    // Primary Key
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Foreign key
    public int ConfigurationId { get; set; }
    public GameConfiguration Configuration { get; set; } = default!;
    
    // public int UserId { get; set; }
    // public User User { get; set; } = default!;
    
    [MaxLength(128)]
    public string SaveName { get; set; } = default!;

    [MaxLength(128)]
    public string PlayerXName { get; set; } = default!;
    
    [MaxLength(128)]
    public string PlayerXSymbol { get; set; } = default!;
    [MaxLength(128)]
    public string PlayerOName { get; set; } = default!;
    
    [MaxLength(128)]
    public string PlayerOSymbol { get; set; } = default!;
    
    public int NextMoveBy { get; set; }
    public int MoveCount { get; set; }
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    [MaxLength(10240)]
    public string GameBoardSerialized { get; set; } = default!;
}