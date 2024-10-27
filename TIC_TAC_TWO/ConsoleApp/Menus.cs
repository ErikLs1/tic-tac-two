using MenySystem;

namespace ConsoleApp;

public static class Menus
{
    public static readonly Menu OptionsMenu =
        new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TOE Information", [
                new MenuItem()
                {
                    Shortcut = "X",
                    Title = "X Starts",
                    MenuItemAction = DummyMethod
                },
                new MenuItem()
                {
                    Shortcut = "O",
                    Title = "O Starts",
                    MenuItemAction = DummyMethod
                },
            ]);

    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "TIC-TAC-TOE", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "New game",
                MenuItemAction = GameController.MainLoop
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
                Shortcut = "R",
                Title = "About the game rules",
                MenuItemAction = AboutTheGameRulesPage
            }
        ]);

    private static string DummyMethod()
    {
        Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
        Console.ReadKey();
        return "foobar";
    }

    public static string AboutUsPage()
    {
        Console.WriteLine("\x1b[1mDeveloper of the game:\x1b[0m Erik Lihhats");
        Console.WriteLine("\x1b[1mPlace of study:\x1b[0m Tallinn University of Technology");
        Console.WriteLine("\x1b[1mBackground:\x1b[0m Professional Swimmer (12+ years)🏊‍♂️");
        Console.WriteLine("\x1b[1mHobbies:\x1b[0m ");
        Console.WriteLine("     * Gym 🏋️💪");
        Console.WriteLine("     * Muay Thai 🥊");
        Console.WriteLine("     * Chess ♟️");
        Console.WriteLine("\x1b[1mLanguages:\x1b[0m ");
        Console.WriteLine("     * English (fluent) 🇬🇧");
        Console.WriteLine("     * Estonia (fluent) 🇪🇪");
        Console.WriteLine("     * Russian (fluent) 🇷🇺");
        
        return "";
    }
    
    public static string AboutTheGamePage()
    {
        Console.WriteLine("Tic-Tac-Two is a variation of tic-tac-toe ");
        Console.WriteLine("published by Chicago-based game seller Marbles: The Brain Store.");
        Console.WriteLine("The objective of creating a three-in-a-row is the same,");
        Console.WriteLine("but players are also allowed to move the tic-tac-toe grid and the markers.");
        return "";
    }

    
    public static string AboutTheGameRulesPage()
    {
        Console.WriteLine("\x1b[1mClassical Game Rules\x1b[0m");
        Console.WriteLine("\x1b[1m====================\x1b[0m");
        
        Console.WriteLine("At the start of the game, each player takes turns placing one of their pieces");
        Console.WriteLine("on any empty cell contained within the tic-tac-toe grid.");
        Console.WriteLine();
        
        Console.WriteLine("Once each player has placed at least two of their pieces, they may do one of three things on their turn: ");
        Console.WriteLine("     1. place one of their remaining pieces on an empty cell within the tic-tac-toe grid,");
        Console.WriteLine("     2. move the tic-tac-toe grid such that it is centered at a cell one space horizontally,");
        Console.WriteLine("        vertically, or diagonally away from the cell it was originally centered at,");
        Console.WriteLine("     3. move one of their pieces that is already on the board (regardless of whether it is");
        Console.WriteLine("        within the tic-tac-toe grid) to any empty cell within the grid.");
        Console.WriteLine();
        
        Console.WriteLine("The first player to create a horizontal, vertical, or diagonal line of their");
        Console.WriteLine("own pieces contained within the tic-tac-toe grid wins.");
        Console.WriteLine("If in a single move the grid has been moved such that it contains ");
        Console.WriteLine("both a three-in-a-row of X pieces and a three-in-a-row of O pieces, then the game is a tie.");
        Console.WriteLine();
        
        Console.WriteLine("\x1b[1mBUT ‼️‼️\x1b[0m");
        Console.WriteLine("\x1b[1m====================\x1b[0m");
        
        Console.WriteLine("In our incredible game you can also choose what ever configuration that you like.");
        Console.WriteLine("You can literally configure any rule. This way you can get more fun with your friends");
        return "";
    }

}