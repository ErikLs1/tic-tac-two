using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    public List<string> GetConfigurationNames()
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
        return context.Configurations
            .Select(c => c.Name)
            .ToList();
        
        //throw new NotImplementedException();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        using var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());

        var config = context.Configurations
            .FirstOrDefault(c => c.Name == name);

        if (config == null)
        {
            throw new Exception("Configuration not found");
        }

        return new GameConfiguration
        {
            Name = config.Name,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            GridWidth = config.GridWidth,
            GridHeight = config.GridHeight,
            WinCondition = config.WinCondition,
            MovePieceAfterNMoves = config.MovePieceAfterNMoves
        };
        //throw new NotImplementedException();
    }

    public void SaveConfiguration(GameConfiguration gameConfig)
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
        // throw new NotImplementedException();
    }
}