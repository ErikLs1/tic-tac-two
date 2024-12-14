using Azure;
using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Custom : PageModel
{
    private readonly IConfigRepository _configRepository;

    public Custom(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }


    [BindProperty]
    public GameConfig GameConfig { get; set; } = default!;
    
    public IActionResult OnGet()
    {
        return Page();
    }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _configRepository.SaveConfiguration(GameConfig);
        var configurations = _configRepository.GetConfigurationNames();
        var newConfig = configurations.FirstOrDefault(c => c.Name == GameConfig.Name);
        if (newConfig == default)
        {
            TempData["Error"] = "Could not find the config";
            return Page();
        }
        
        return RedirectToPage("./Index");
    }
}