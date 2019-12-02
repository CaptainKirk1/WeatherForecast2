using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InterviewProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static APIClient _apiClient = new APIClient();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        private async Task<IEnumerable<WeatherForecast>> GetForecastList()
        {
            int? woeid = await _apiClient.GetWhereOnEarthId("San Francisco");
            
            IEnumerable<WeatherForecast> forecasts = null;
            if (woeid.HasValue)
            {
                forecasts = await _apiClient.GetWeatherData(woeid.Value);
            }
            return forecasts != null ? forecasts : new List<WeatherForecast>();
        }

        [HttpGet]
        public async IAsyncEnumerable<WeatherForecast> Get()
        {
            foreach (var forecast in await GetForecastList())
            {
                yield return forecast;
            }
        }
    }
}
