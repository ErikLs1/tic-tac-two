using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<GameConfiguration> Configurations { get; set; } = default!;
    public DbSet<Game> Games { get; set; } = default!;
    
    //DbSet<User> Users { get; set; }
    // Data Source=/Users/eriklihhats/configurations/app.db
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}