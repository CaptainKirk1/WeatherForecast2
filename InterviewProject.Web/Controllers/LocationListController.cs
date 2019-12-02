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
    public class LocationListController : ControllerBase
    {
        private static APIClient _apiClient = new APIClient();

        private readonly ILogger<WeatherForecastController> _logger;

        public LocationListController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async IAsyncEnumerable<string> Get(string search)
        {
            foreach (var location in await _apiClient.GetLocationList(search))
            {
                yield return location;
            }
        }
    }
}
