// See https://aka.ms/new-console-template for more information


using GameBrain;
using MenuSystem;


var gameInstance = new TicTacToeBrain(4);
var deepMenu = new Menu(
    EMenuLevel.Deep,
    "TIC-TAC-TOE DEEP", [
        new MenuItem()
        {
            Shortcut = "Y",
            Title = "YYY",
            MenuItemAction = DummyMethod
        },
    
        new MenuItem()
        {
            Shortcut = "X",
            Title = "XXXX",
            MenuItemAction = DummyMethod
        }
    ]);
var optionsMenu = new Menu(
    EMenuLevel.Secondary,
    "TIC-TAC-TOE Options", [
    new MenuItem()
    {
        Shortcut = "X",
        Title = "X Starts",
        MenuItemAction = deepMenu.Run
    },
    
    new MenuItem()
    {
        Shortcut = "O",
        Title = "O Stats",
        MenuItemAction = DummyMethod
    }
]);

var mainMenu = new Menu(
    EMenuLevel.Main,
    "TIC-TAC-TOE", [
    new MenuItem()
    {
        Shortcut = "O",
        Title = "Options",
        MenuItemAction = optionsMenu.Run
    },


    new MenuItem()
    {
        Shortcut = "N",
        Title = "New game",
        MenuItemAction = NewGame
    }
]);

mainMenu.Run();

return;
//=================================

string DummyMethod()
{
    Console.WriteLine("Just press any key to get out from here! (Any key - as a random choice from a keyboard!)");
    return "foobar";
}

string NewGame()
{
    ConsoleUI.Visualize.DrawBoard(gameInstance);
    
    Console.Write("Give me coordinates <x, y>: ");
    var input = Console.ReadLine()!;
    var inputSplit = input.Split(",");
    var inputX = int.Parse(inputSplit[0]);
    var inputY = int.Parse(inputSplit[1]);
    gameInstance.MakeAMove(inputX, inputY);
    
    // loop
    // draw the board again
    // ask input again
    // is game over?
    
    return "";
}
