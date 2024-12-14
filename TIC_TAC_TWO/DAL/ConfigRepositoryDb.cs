using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    public List<(int Id, string Name)> GetConfigurationNames()
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var configList = context.Configurations
            .Select(c => new { c.Id, c.Name })
            .ToList(); 

        return configList
            .Select(c => (c.Id, c.Name))
            .ToList();
    }

    public GameConfig GetConfigurationById(int id)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());

        var config = context.Configurations
            .FirstOrDefault(c => c.Id == id);

        if (config == null)
        {
            throw new Exception($"Configuration with ID {id} not found.");
        }

        return new GameConfig
        {
            Name = config.Name,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            GridWidth = config.GridWidth,
            GridHeight = config.GridHeight,
            WinCondition = config.WinCondition,
            MovePieceAfterNMoves = config.MovePieceAfterNMoves
        };
    }

    public int SaveConfigurationAndGetId(GameConfig gameConfig)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var existingConfig = context.Configurations.FirstOrDefault(c => c.Name == gameConfig.Name);
        if (existingConfig != null)
        {
            existingConfig.BoardSizeWidth = gameConfig.BoardSizeWidth;
            existingConfig.BoardSizeHeight = gameConfig.BoardSizeHeight;
            existingConfig.GridWidth = gameConfig.GridWidth;
            existingConfig.GridHeight = gameConfig.GridHeight;
            existingConfig.WinCondition = gameConfig.WinCondition;
            existingConfig.MovePieceAfterNMoves = gameConfig.MovePieceAfterNMoves;
            return existingConfig.Id;
        }
        else
        {
            var config = new Domain.GameConfiguration
            {
                Name = gameConfig.Name,
                BoardSizeWidth = gameConfig.BoardSizeWidth,
                BoardSizeHeight = gameConfig.BoardSizeHeight,
                GridWidth = gameConfig.GridWidth,
                GridHeight = gameConfig.GridHeight,
                WinCondition = gameConfig.WinCondition,
                MovePieceAfterNMoves = gameConfig.MovePieceAfterNMoves,
            };
            context.Configurations.Add(config);
            context.SaveChanges();
            return config.Id;
        }
    }

    public void SaveConfiguration(GameConfig gameConfig)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        var existingConfig = context.Configurations.FirstOrDefault(c => c.Name == gameConfig.Name);
        if (existingConfig != null)
        {
            existingConfig.BoardSizeWidth = gameConfig.BoardSizeWidth;
            existingConfig.BoardSizeHeight = gameConfig.BoardSizeHeight;
            existingConfig.GridWidth = gameConfig.GridWidth;
            existingConfig.GridHeight = gameConfig.GridHeight;
            existingConfig.WinCondition = gameConfig.WinCondition;
            existingConfig.MovePieceAfterNMoves = gameConfig.MovePieceAfterNMoves;
        }
        else
        {
            var config = new Domain.GameConfiguration
            {
                Name = gameConfig.Name,
                BoardSizeWidth = gameConfig.BoardSizeWidth,
                BoardSizeHeight = gameConfig.BoardSizeHeight,
                GridWidth = gameConfig.GridWidth,
                GridHeight = gameConfig.GridHeight,
                WinCondition = gameConfig.WinCondition,
                MovePieceAfterNMoves = gameConfig.MovePieceAfterNMoves,
            };
            context.Configurations.Add(config);
        }

        context.SaveChanges();
    }
}