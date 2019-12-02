using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;

namespace InterviewProject
{
    public class APIClient
    {
        const string ROOT_URL = "https://www.metaweather.com/api/";
        static HttpClient client = new HttpClient();

        private async Task<string> ExecuteGet(string methodAndQuery)
        {
            string url = $"{ROOT_URL}{methodAndQuery}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IEnumerable<string>> GetLocationList(string searchValue)
        {
            string responseString = await ExecuteGet($"location/search/?query={searchValue}");
            var document = JsonDocument.Parse(responseString);
            var list = document.RootElement.EnumerateArray()
                .Select(element => element.GetProperty("title").ToString())
                .ToList();
            return list;
        }

        public async Task<int?> GetWhereOnEarthId(string locationName)
        {
            string responseString = await ExecuteGet($"location/search/?query={locationName}");
            var document = JsonDocument.Parse(responseString);
            var element = document.RootElement.EnumerateArray().FirstOrDefault();
            if (!EqualityComparer<JsonElement>.Default.Equals(element, default)
                && !string.IsNullOrWhiteSpace(element.GetRawText()))
            {
                return element.GetProperty("woeid").GetInt32();
            }
            return null;
        }

        public async Task<IEnumerable<WeatherForecast>>GetWeatherData(int whereOnEarthId)
        {
            string responseString = await ExecuteGet($"location/{whereOnEarthId}/");
            var document = JsonDocument.Parse(responseString);

            DateTime getDateTime(string dateTime)
            {
                DateTime dt;
                if (DateTime.TryParse(dateTime, out dt))
                {
                    return dt;
                }
                return DateTime.MinValue;
            }

            WeatherForecast getWeatherForecast(JsonElement element)
            {
                string summary = element.GetProperty("weather_state_name").GetString();
                string abbreviation = element.GetProperty("weather_state_abbr").GetString();
                string dateString = element.GetProperty("applicable_date").GetString();
                DateTime date = getDateTime(dateString);
                double tempCdouble = element.GetProperty("the_temp").GetDouble();
                int tempC = Convert.ToInt32(Math.Round(tempCdouble, 0, MidpointRounding.AwayFromZero));

                return new WeatherForecast
                {
                    Summary = summary,
                    Abbreviation = abbreviation,
                    Date = date.ToShortDateString(),
                    TemperatureC = tempC
                };
            }

            return document.RootElement.GetProperty("consolidated_weather")
                .EnumerateArray()
                .Select(element => getWeatherForecast(element))
                .ToList();
        }


    }
}
