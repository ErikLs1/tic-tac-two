﻿using GameBrain;

namespace DAL;

public class ConfigRepository
{
    private List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration()
        {
            Name = "Classical",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            WinCondition = 3,
            MovePieceAfterNMoves = 2
        },
        new GameConfiguration()
        {
            Name = "Big board",
            BoardSizeWidth = 10,
            BoardSizeHeight = 10,
            WinCondition = 4,
            MovePieceAfterNMoves = 3,
        },
    };

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }

}