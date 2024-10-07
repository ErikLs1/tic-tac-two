using MenySystem;

namespace ConsoleApp;

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
                    Title = "O Starts",
                    MenuItemAction = DummyMethod
                },
            ]);

    public static Menu MainMenu = new Menu(
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

    private static string DummyMethod()
    {
        Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
        Console.ReadKey();
        return "foobar";
    }

}