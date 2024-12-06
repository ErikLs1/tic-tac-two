using DAL;
using MenySystem;

namespace ConsoleApp;

public static class Menus
{
    // private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryJson();
    // private static readonly IGameRepository GameRepository = new GameRepositoryJson();
    private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryDb();
    private static readonly IGameRepository GameRepository = new GameRepositoryDb();
    
    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m \ud83d\ude00\ud83d\ude00", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "New game",
                MenuItemAction = () => GameController.MainLoop(ConfigRepository, GameRepository)
            },
            new MenuItem()
            {
                Shortcut = "L",
                Title = "Saved Games",
                MenuItemAction = LoadSavedGamesMenu
            },
            
            new MenuItem()
            {
                Shortcut = "A",
                Title = "About Us",
                MenuItemAction = AboutUsPage
            },
            
            new MenuItem()
            {
                Shortcut = "G",
                Title = "About the game",
                MenuItemAction = AboutTheGamePage
            },
            
            new MenuItem()
            {
                Shortcut = "S",
                Title = "About the game rules",
                MenuItemAction = AboutTheGameRulesPage
            }
        ]);

    public static string LoadSavedGamesMenu()
    {
        var savedGameNames = GameRepository.GetSavedGames();

        if (savedGameNames.Count == 0)
        {
            Console.WriteLine("No saved games found.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            return MainMenu.Run();
        }

        var menuItems = new List<MenuItem>();
        int index = 1;

        foreach (var savedGame in savedGameNames)
        {
            var displayName = Path.GetFileNameWithoutExtension(savedGame);
            var menuItem = new MenuItem()
            {
                Shortcut = index.ToString(),
                Title = displayName,
                MenuItemAction = () =>
                {
                    return SavedGameOptionsMenu(savedGame);
                }
            };
            menuItems.Add(menuItem);
            index++;
        }
        
        menuItems.Add(new MenuItem()
        {
            Shortcut = "R",
            Title = "Return",
            MenuItemAction = () => MainMenu.Run()
        });

        var savedGamesMenu = new Menu(
            EMenuLevel.Secondary,
            "\x1b[1m\x1b[35mSaved Games\x1b[0m\x1b[0m",
            menuItems
        );

        return savedGamesMenu.Run();
    }

    public static string SavedGameOptionsMenu(string savedGameName)
    {
        var menuItems = new List<MenuItem>()
        {
            new MenuItem()
            {
                Shortcut = "I",
                Title = "Load Game",
                MenuItemAction = () =>
                {
                    var gameState = GameRepository.LoadGame(savedGameName);
                    GameController.MainLoopWithLoadedGame(gameState, ConfigRepository, GameRepository);
                    return "";
                }
            },
            
            new MenuItem()
            {
                Shortcut = "D",
                Title = "Delete Game",
                MenuItemAction = () =>
                {
                    GameRepository.DeleteGame(savedGameName);
                    Console.WriteLine("Game deleted.");
                    // Console.WriteLine(FileHelper.BasePath + savedGameName);
                    Console.WriteLine("Press any key to return back.");
                    Console.ReadKey();
                    return LoadSavedGamesMenu();
                }
            },
            
            new MenuItem()
            {
                Shortcut = "R",
                Title = "Return",
                MenuItemAction = LoadSavedGamesMenu
            },
            
            new MenuItem()
            {
                Shortcut = "E",
                Title = "Exit",
                MenuItemAction = LoadSavedGamesMenu
            }
        };

        var savedGamesOptionsMenu = new Menu(
            EMenuLevel.Secondary,
            "\x1b[1m\x1b[35mSaved Games\x1b[0m\x1b[0m" + $"\x1b[1m\x1b[35m - {savedGameName}\x1b[0m\x1b[0m",
            menuItems
        );
        return savedGamesOptionsMenu.Run();
    }

    public static string AboutUsPage()
    {
        var aboutUsContent = 
        "\x1b[1m\x1b[32mDeveloper of the game\x1b[0m\x1b[0m: Erik Lihhats\n" +
            "\x1b[1m\x1b[32mPlace of study\x1b[0m\x1b[0m: Tallinn University of Technology\n" +
            "\x1b[1m\x1b[32mBackground\x1b[0m\x1b[0m: Professional Swimmer (12+ years)🏊‍♂️\n" +
            "\x1b[1m\x1b[32mHobbies\x1b[0m\x1b[0m: \n" +
            "     * Gym 🏋️💪\n" +
            "     * Muay Thai 🥊\n" +
            "     * Chess ♟️\n" +
            "\x1b[1m\x1b[32mLanguages\x1b[0m\x1b[0m:\n" +
            "     * English (fluent) 🇬🇧\n" +
            "     * Estonian (fluent) 🇪🇪\n" +
            "     * Russian (fluent) 🇷🇺\n";
        
        var aboutUsMenu = new Menu(
            EMenuLevel.Secondary,  
            "\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m",
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Shortcut = "R",
                    Title = "Return",
                    MenuItemAction = () => MainMenu.Run()
                },
            },
            aboutUsContent 
        );

        return aboutUsMenu.Run();
    }
    
    public static string AboutTheGamePage()
    {
        var aboutTheGameContent = "\nTic-Tac-Two is a variation of tic-tac-toe\n" +
                                  "published by Chicago-based game seller Marbles: The Brain Store.\n" +
                                  "The objective of creating a three-in-a-row is the same,\n" +
                                  "but players are also allowed to move the tic-tac-toe grid and the markers.\n";
        
        var aboutTheGameMenu = new Menu(
            EMenuLevel.Secondary,  
            "\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m",
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Shortcut = "R",
                    Title = "Return",
                    MenuItemAction = () => MainMenu.Run()
                },
            },
            aboutTheGameContent 
        );


        return aboutTheGameMenu.Run();
    }

    
    public static string AboutTheGameRulesPage()
    {
        var aboutTheGameRulesContent = "\nAt the start of the game, each player takes turns placing one of their pieces\n" +
                                       "on any empty cell contained within the tic-tac-toe grid.\n\n" +
                                       "Once each player has placed at least two of their pieces, they may do one of three things on their turn: \n" +
                                       "     1. place one of their remaining pieces on an empty cell within the tic-tac-toe grid,\n" +
                                       "     2. move the tic-tac-toe grid such that it is centered at a cell one space horizontally,\n" +
                                       "        vertically, or diagonally away from the cell it was originally centered at,\n" +
                                       "     3. move one of their pieces that is already on the board (regardless of whether it is\n" +
                                       "        within the tic-tac-toe grid) to any empty cell within the grid.\n\n" +
                                       "The first player to create a horizontal, vertical, or diagonal line of their\n" +
                                       "own pieces contained within the tic-tac-toe grid wins.\n" +
                                       "If in a single move the grid has been moved such that it contains \n" +
                                       "both a three-in-a-row of X pieces and a three-in-a-row of O pieces, then the game is a tie.\n\n" +
                                       "\x1b[1mBUT\x1b[0m ‼\ufe0f‼\ufe0f‼\ufe0f‼\ufe0f \n\n" +
                                       "In our incredible game you can also choose what ever configuration that you like.\n" +
                                       "You can literally configure any rule. This way you can get more fun with your friends\n";
        
        var aboutTheGameRulesMenu = new Menu(
            EMenuLevel.Secondary,  
            "\x1b[1m\x1b[35mTIC-TAC-TWO\x1b[0m\x1b[0m",
            new List<MenuItem>
            {
                new MenuItem()
                {
                    Shortcut = "R",
                    Title = "Return",
                    MenuItemAction = () => MainMenu.Run()
                },
            },
            aboutTheGameRulesContent 
        );
        return aboutTheGameRulesMenu.Run();
    }

}