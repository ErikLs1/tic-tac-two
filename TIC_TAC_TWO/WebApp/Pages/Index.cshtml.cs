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
    
    public SelectList ConfigSelectList { get; set; } = default!;

    [BindProperty] 
    public int ConfigId { get; set; }

    public IActionResult OnGet()
    {
        var selectListData = _configRepository.GetConfigurationNames()
            .Select(name => new {id = name, value = name})
            .ToList();
        
        ConfigSelectList = new SelectList(selectListData, "id", "value");
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            var selectListData = _configRepository.GetConfigurationNames()
                .Select(name => new {id = name, value = name})
                .ToList();
        
            ConfigSelectList = new SelectList(selectListData, "id", "value");
        }
        
        return RedirectToPage("./PlayGame", new { ConfigId = ConfigId });
    }
}