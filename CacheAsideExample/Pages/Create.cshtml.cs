using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CacheAsideExampel.Data;
using CacheAsideExample.Services;

namespace CacheAsideExample.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ProjectContext _context;
        private readonly RedisCacheAside _redisCacheAside;

        public CreateModel(ProjectContext context, RedisCacheAside redisCacheAside)
        {
            _context = context;
            _redisCacheAside = redisCacheAside;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ExchangeRate ExchangeRate { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.ExchangeRates == null || ExchangeRate == null)
            {
                return Page();
            }

            _context.ExchangeRates.Add(ExchangeRate);
            await _context.SaveChangesAsync();

            await _redisCacheAside.Invalidate("Rates");

            return RedirectToPage("/WithoutCache");
        }
    }
}
