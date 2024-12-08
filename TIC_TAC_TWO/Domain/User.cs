using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class User
{
    // Primary Key
    public int Id { get; set; }

    [MaxLength(128)]
    public string Username { get; set; } = default!;
    
    [MaxLength(256)]
    public string Password { get; set; } = default!;

    public ICollection<Game>? GameAsPlayerX { get; set; }
    public ICollection<Game>? GameAsPlayerO { get; set; }
}