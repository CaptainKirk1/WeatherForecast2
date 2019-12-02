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


    }
}
