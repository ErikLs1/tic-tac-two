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
        [BindProperty(SupportsGet = true)] public string Username { get; set; } = default!;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Game = await _context.Games
                .Include(g => g.Configuration).ToListAsync();
        }
    }
}
