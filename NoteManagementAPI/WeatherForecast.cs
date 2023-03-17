using System.Linq.Expressions;

namespace NoteManagementAPI
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        public Expression<Func<WeatherForecast, bool>> Filter { get; set; } = null;
        public Func<IQueryable<WeatherForecast>, IOrderedQueryable<WeatherForecast>> OrderBy { get; set; } = null;
        public string includeProperties { get; set; } = "";
    }
}