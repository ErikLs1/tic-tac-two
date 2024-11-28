using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    // Primary Key
    public int Id { get; set; }

    [MaxLength(128)]
    public string Username { get; set; } = default!;
    
    [MaxLength(256)]
    public string Password { get; set; } = default!;
    
    public ICollection<Game>? Games { get; set; }
}