using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? UserName { get; set; }
    
        [BindProperty(SupportsGet = true)]
        public bool IsAdmin { get; set; }
        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            { 
                RedirectToPage("./Index", new { error = "No username provided" });
            }
            
            ViewData["UserName"] = UserName;
            ViewData["IsAdmin"] = IsAdmin;
            
            Game = await _context.Games
                .Include(g => g.PlayerO)
                .Include(g => g.PlayerX).ToListAsync();
        }
    }
}
