using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<GameConfiguration> Configurations { get; set; } = default!;
    
    // DbSet<User> Users { get; set; }
    // Data Source=/Users/eriklihhats/configurations/app.db
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Configuration)
            .WithOne(gc => gc.Game)
            .HasForeignKey<GameConfiguration>(gc => gc.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerX)
            .WithMany(u => u.GameAsPlayerX)
            .HasForeignKey(g => g.PlayerXId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerO)
            .WithMany(u => u.GameAsPlayerO)
            .HasForeignKey(g => g.PlayerOId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}