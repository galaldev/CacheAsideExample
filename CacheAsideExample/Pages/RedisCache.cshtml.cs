using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CacheAsideExample.Dtos;
using CacheAsideExampel.Data;
using CacheAsideExample.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CacheAsideExample.Pages
{
    public class RedisCacheModel : PageModel
    {
        private readonly ProjectContext _projectContext;
        private readonly RedisCacheAside _redisCacheAside;

        public List<CurrentRateDto> CurrentRates { get; set; }
        public long ElapsedMilliseconds { get; private set; }
        public RedisCacheModel(ProjectContext projectContext, RedisCacheAside redisCacheAside)
        {
            _projectContext = projectContext;
            _redisCacheAside = redisCacheAside;
        }

        public async Task OnGet()
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            CurrentRates = await _redisCacheAside.Get("Rates", 
                () => _projectContext.ExchangeRates.GroupBy(c => new
                        {
                            c.Currency
                        }).Select(c => new CurrentRateDto
                        {
                            Currency = c.Key.Currency,
                            Date = c.OrderByDescending(x => x.Date).Select(x => x.Date).First(),
                            Rate = c.OrderByDescending(x => x.Date).Select(x => x.Rate).First()
                        }).ToListAsync()
                );
            stopwatch.Stop();
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        }
    }
}