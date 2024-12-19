using GameBrain;

namespace DAL;

public class ConfigRepositoryHardcoded
{
    private static List<GameConfig> _gameConfigurations = new List<GameConfig>()
    {
        new GameConfig()
        {
            Name = "Classical (5x5)"
        },
        new GameConfig()
        {
            Name = "Big board (10x10)",
            BoardSizeWidth = 10,
            BoardSizeHeight = 10,
            WinCondition = 4,
            GridWidth = 3,
            GridHeight = 3,
            MovePieceAfterNMoves = 3,
        },
    };

    public static List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public static GameConfig GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }

    // public static GameConfig ConfigureCustomGame()
    // {
    //     var config = new GameConfig();
    //     
    //     Console.Clear();
    //     Console.WriteLine("\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m");
    //     Console.WriteLine("\x1b[1m\x1b[36m====================\x1b[0m\x1b[0m");
    //
    //     Console.Write("Enter the name for the config: ");
    //     var nameInput = Console.ReadLine()?.Trim();
    //
    //     while (string.IsNullOrWhiteSpace(nameInput))
    //     {
    //         Console.WriteLine("Invalid input. Please enter a non-empty name:");
    //         nameInput = Console.ReadLine()?.Trim();
    //     }
    //
    //     config.Name = nameInput;
    //     config.BoardSizeWidth = CheckForValidInput("Enter the board width (minimum 3, maximum 25): ",3, 25);
    //     config.BoardSizeHeight = CheckForValidInput("Enter the board height (minimum 3, maximum 25): ",3, 25);
    //     config.GridWidth = CheckForValidInput($"Enter the grid width (minimum 3, max {config.BoardSizeWidth}): ",3, config.BoardSizeWidth);
    //     config.GridHeight = CheckForValidInput($"Enter the grid height (minimum 3, max {config.BoardSizeHeight}): ",3, config.BoardSizeHeight);
    //     config.WinCondition = CheckForValidInput($"Enter the winning condition (minimum 3): ",3);
    //     config.MovePieceAfterNMoves = CheckForValidInput($"Enter after how many moves you can choose to move your piece (minimum 2): ",2);
    //     
    //     return config;
    // }
    //
    // private static int CheckForValidInput(string prompt, int min, int? max = null)
    // {
    //     int inputValue;
    //
    //     while (true) 
    //     {
    //         Console.WriteLine(prompt);
    //         var input = Console.ReadLine()!;
    //         
    //         if (int.TryParse(input, out inputValue) && inputValue >= min && (max == null || inputValue <= max))
    //         {
    //             return inputValue;
    //         }
    //         
    //         Console.WriteLine($"Invalid input. Please enter a valid integer between {min} and {(max.HasValue ? max.Value.ToString() : "~")}.");
    //     }
    // }
    
    
}