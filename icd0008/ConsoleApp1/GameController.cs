using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp1;

public static class GameController
{
    private static readonly ConfigRepository ConfigRepository = new ConfigRepository();
    public static string MainLoop()
    {
        var chooseConfigShortcut = ChooseConfiguration();
        if (!int.TryParse(chooseConfigShortcut, out var configNo))
        {
            return chooseConfigShortcut;
        }
    
        var chooseConfig = ConfigRepository.GetConfigurationByName(
            ConfigRepository.GetConfigurationNames()[configNo]);
        var gameInstance = new TicTacToeBrain(chooseConfig);

        do
        {
            ConsoleUI.Visualize.DrawBoard(gameInstance);
    
            Console.Write("Give me coordinates <x, y>: ");
            var input = Console.ReadLine()!;
            var inputSplit = input.Split(",");
            var inputX = int.Parse(inputSplit[0]);
            var inputY = int.Parse(inputSplit[1]);
            gameInstance.MakeAMove(inputX, inputY);
        } while (true);
    }

    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = ConfigRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }
    
        var configMenu = new Menu(EMenuLevel.Deep, 
            "TIC-TAC-TOW - Choose game configuration", 
            configMenuItems);
        return configMenu.Run();
    }
}