namespace MenySystem;

public class Menu
{
    private string MenuHeader { get; set; }
    private static string _menuDivider = "\x1b[1m\x1b[36m=================\x1b[0m\x1b[0m";
    private HashSet<MenuItem> MenuItems { get; set; }
    private string? MenuContent { get; set; }

    private MenuItem _menuItemExit = new MenuItem()
    {
        Shortcut = "E",
        Title = "Exit",
    };

    private MenuItem _menuItemReturn = new MenuItem()
    {
        Shortcut = "R",
        Title = "Return"
    };

    private MenuItem _menuItemReturnMain = new MenuItem()
    {
        Shortcut = "M",
        Title = "Return to main menu",
    };

    private EMenuLevel _menuLevel { get; set; }
    private bool _isCustomMenu { get; set; }

    public void SetMenuItemAction(string shortcut, Func<string> action)
    {
        var menuItem = MenuItems.Single(m => m.Shortcut == shortcut);
        menuItem.MenuItemAction = action;
    }
    
    public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems, string? menuContent = null, bool isCustomMenu = false)
    {
        if (string.IsNullOrWhiteSpace(menuHeader))
        {
            throw new ApplicationException("Menu header cannot be empty.");
        }
        
        MenuHeader = menuHeader;
        
        if (menuItems == null || menuItems.Count == 0)
        {
            throw new ApplicationException("Menu items cannot be empty.");
        }

        MenuItems = new HashSet<MenuItem>(menuItems, new MenuItemComparer());
        _isCustomMenu = isCustomMenu;
        _menuLevel = menuLevel;
        MenuContent = menuContent;
        
        
        if (_menuLevel != EMenuLevel.Main)
        {
            MenuItems.Add(_menuItemReturn);
        }
        
        if (_menuLevel == EMenuLevel.Deep)
        {
            MenuItems.Add(_menuItemReturnMain);
        }

        MenuItems.Add(_menuItemExit);
    }

    public string Run()
    {
        Console.Clear();
        do
        {
            var menuItem = DisplayMenuGetUserChoice();
            var menuReturnValue = "";

            if (menuItem.MenuItemAction != null)
            {
                menuReturnValue = menuItem.MenuItemAction();
            }
            
            if (menuItem.Shortcut == _menuItemReturn.Shortcut)
            {
                return menuItem.Shortcut;
            }
            
            if (menuItem.Shortcut == _menuItemExit.Shortcut || menuReturnValue == _menuItemExit.Shortcut)
            {
                return _menuItemExit.Shortcut;
            }

            if ((menuItem.Shortcut == _menuItemReturnMain.Shortcut || menuReturnValue == _menuItemReturnMain.Shortcut) && _menuLevel != EMenuLevel.Main)
            {
                return _menuItemReturnMain.Shortcut;
            }

            if (!string.IsNullOrWhiteSpace(menuReturnValue))
            {
                return menuReturnValue;
            }
        } while (true);
    }       
    
    public MenuItem DisplayMenuGetUserChoice()
    {
        var userInput = "";

        do
        {
            DrawMenu();

            userInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Please, choose the option");
                Console.WriteLine();
            }
            else
            {
                userInput = userInput.ToUpper();
                
                foreach (var item in MenuItems)
                {
                    if (item.Shortcut.ToUpper() != userInput) continue;
                    return item;
                }
                
                Console.WriteLine("Please, choose something from existing list of options.");
                Console.WriteLine();
            }
        } while (true);
    }
    private void DrawMenu()
    {
        Console.WriteLine(MenuHeader);
        Console.WriteLine(_menuDivider);
        
        if (!string.IsNullOrWhiteSpace(MenuContent))
        {
            Console.WriteLine(MenuContent);
            Console.WriteLine(_menuDivider);
        }
        
        foreach (var item in MenuItems)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();
        Console.Write(">");
    }
}
