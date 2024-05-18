using Newtonsoft.Json;
using Shared.Models.OpenAq.Countries;
using Shared.Models.OpenAq.Locations;
using Shared.Models.OpenAq.Sensors;
using Shared.Models.OpenAq.ViewData;
using System.Net.Http.Headers;

namespace Server.Services
{
    public class OpenAqService : IOpenAqService
    {
        private readonly HttpClient _client;

        public OpenAqService()
        {
            _client = new HttpClient();
        }

        public async Task<GetCountriesResponse> GetCountriesAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.openaq.org/v3/countries?order_by=id&sort_order=asc&limit=100&page=1"),
                Headers =
                {
                    { "accept", "application/json" },
                },
            };
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                GetCountriesResponse getCountriesResponse = JsonConvert.DeserializeObject<GetCountriesResponse>(body);
                return getCountriesResponse;
            }
        }

        public async Task<GetLocationsResponse> GetLocationsAsync(int countryId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.openaq.org/v3/locations?order_by=id&sort_order=asc&countries_id={countryId}&limit=100&page=1"),
                Headers =
                    {
                        { "accept", "application/json" },
                    },
            };
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                GetLocationsResponse getLocationsResponse = JsonConvert.DeserializeObject<GetLocationsResponse>(body);
                return getLocationsResponse;
            }
        }

        public async Task<GetSensorsByLocationIdResponse> GetSensorsByLocationIdAsync(int locationId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.openaq.org/v3/locations/{locationId}/sensors"),
                Headers =
                {
                    { "accept", "application/json" },
                },
            };
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                GetSensorsByLocationIdResponse getSensorsByLocationIdResponse = JsonConvert.DeserializeObject<GetSensorsByLocationIdResponse>(body);
                return getSensorsByLocationIdResponse;
            }
        }

        public async Task<GetViewDataResponse> GetViewDataAsync(int countryId)
        {
            var locations = await GetLocationsAsync(countryId);

            //List<Shared.Models.OpenAq.Sensors.Result> 

            List<List<Shared.Models.OpenAq.Sensors.Result>> allLocationInfo = new List<List<Shared.Models.OpenAq.Sensors.Result>>();

            foreach (var loc in locations.results)
            {
                var sensors = await GetSensorsByLocationIdAsync(loc.id);
                allLocationInfo.Add(sensors.results);
            }
            return null;
        }
    }
}
