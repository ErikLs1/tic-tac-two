using GameBrain;
using MenuSystem;

namespace ConsoleApp1;

public static class Menus
{
    public static readonly Menu OptionsMenu =
         new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TOE Options", [
                new MenuItem()
                {
                    Shortcut = "X",
                    Title = "X Starts",
                    MenuItemAction = DummyMethod
                },
        
                new MenuItem()
                {
                    Shortcut = "O",
                    Title = "O Stats",
                    MenuItemAction = DummyMethod
                }
            ]);

        public static readonly Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "TIC-TAC-TOE", [
                new MenuItem()
                {
                    Shortcut = "O",
                    Title = "Options",
                    MenuItemAction = OptionsMenu.Run
                },


                new MenuItem()
                {
                    Shortcut = "N",
                    Title = "New game",
                    MenuItemAction = GameController.MainLoop
                }
            ]);
    
    public static string DummyMethod()
    {
        Console.WriteLine("Just press any key to get out from here! (Any key - as a random choice from a keyboard!)");
        return "foobar";
    }
}