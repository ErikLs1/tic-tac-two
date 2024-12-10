using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class SelectConfiguration : PageModel
{
    private readonly IConfigRepository _configRepository;

    public SelectConfiguration(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public List<string> Configurations { get; set; }

    [BindProperty]
    public string SelectedConfiguration { get; set; } = default!;
    
    public void OnGet()
    {
        Configurations = _configRepository.GetConfigurationNames();
    }

    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(SelectedConfiguration))
        {
            TempData.Remove("GameState");

            return RedirectToPage("./PlayGame", new { configName = SelectedConfiguration });
        }

        return Page();
    }
}