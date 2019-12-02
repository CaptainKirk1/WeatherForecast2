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

        public async Task<IEnumerable<string>> SearchLocation(string search)
        {
            string responseString = await ExecuteGet($"location/search/?query={search}");
            var document = JsonDocument.Parse(responseString);
            var list = document.RootElement.EnumerateArray()
                .Select(element => element.GetProperty("title").ToString())
                .ToList();
            return list;
        }


    }
}
