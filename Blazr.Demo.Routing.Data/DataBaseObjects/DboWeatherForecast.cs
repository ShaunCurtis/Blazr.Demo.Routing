/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Demo.Routing.Data
{
    internal record DboWeatherForecast
    {
        public Guid Id { get; init; }

        public DateTime Date { get; init; }

        public int TemperatureC { get; init; }

        public string? Summary { get; init; }

        public WeatherForecast ToDto()
            => new WeatherForecast
            {
                Id = this.Id,
                Date = this.Date,
                TemperatureC = this.TemperatureC,
                Summary = this.Summary
            };

        public static DboWeatherForecast FromDto(WeatherForecast record)
            => new DboWeatherForecast
            {
                Id = record.Id,
                Date = record.Date,
                TemperatureC = record.TemperatureC,
                Summary = record.Summary
            };
    }
}
