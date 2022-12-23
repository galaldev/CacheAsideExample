using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CacheAsideExample.Dtos;
using CacheAsideExampel.Data;
using CacheAsideExample.Services;
using System.Diagnostics;

namespace CacheAsideExample.Pages
{
    public class WithoutCacheModel : PageModel
    {
        private readonly ProjectContext _projectContext;

        public List<CurrentRateDto> CurrentRates { get; set; }
        public long ElapsedMilliseconds { get; private set; }

        public WithoutCacheModel(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public void OnGet()
        {
          
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            CurrentRates = _projectContext.
                ExchangeRates.GroupBy(c => new
            {
                c.Currency
            }).Select(c => new CurrentRateDto
            {
                Currency = c.Key.Currency,
                Date = c.OrderByDescending(x => x.Date).Select(x => x.Date).First(),
                Rate = c.OrderByDescending(x => x.Date).Select(x => x.Rate).First()
            }).ToList();
            stopwatch.Stop();
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }
    }
}