using System.Security.Cryptography.X509Certificates;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfigRepository _configRepository;

    public IndexModel(ILogger<IndexModel> logger, IConfigRepository configRepository)
    {
        _logger = logger;
        _configRepository = configRepository;
    }

    public SelectList? ConfigSelectList { get; set; }
    
    [BindProperty]
    public int ConfigId { get; set; }
    public IActionResult OnGet()
    {
        var selectListData = _configRepository.GetConfigurationNames()
            .Select(config => new
            {
                Id = config.Id, 
                Name = config.Name
            })
            .ToList();
        ConfigSelectList = new SelectList(selectListData, "Id", "Name");
        return Page();
    }

    public IActionResult OnPost(string action)
    {
        if (action == "customConfig")
        {
            return RedirectToPage("./Custom");
        }

        if (action == "createGame")
        {
            if (!ModelState.IsValid || ConfigId == 0)
            {
                var selectListData = _configRepository.GetConfigurationNames()
                    .Select(config => new
                    {
                        Id = config.Id, 
                        Name = config.Name
                    })
                    .ToList();
                ConfigSelectList = new SelectList(selectListData, "Id", "Name");
                return Page();
            }
            return RedirectToPage("./PlayGame", new {ConfigId});
        }

        return Page();
    }
}