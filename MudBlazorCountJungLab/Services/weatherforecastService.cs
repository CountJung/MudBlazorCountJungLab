using Microsoft.Extensions.Caching.Memory;
using static MudBlazorCountJungLab.Pages.FetchData;

namespace MudBlazorCountJungLab.Services
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public class weatherforecastService
    {
        private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public weatherforecastService(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public IMemoryCache MemoryCache { get; }

        public Task<WeatherForecast[]?> GetForecastAsync(DateTime startDate)
        {
            return MemoryCache.GetOrCreateAsync(startDate, async e =>
            {
                e.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(30)
                });

                var rng = new Random();

                await Task.Delay(TimeSpan.FromSeconds(10));

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = startDate.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summaries[rng.Next(summaries.Length)]
                }).ToArray();
            });
        }
        public static List<WeatherForecast>? GetForecasts(DateTime startDate) 
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            }).ToList();
        }
    }
}
