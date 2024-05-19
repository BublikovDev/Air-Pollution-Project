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
                RequestUri = new Uri($"https://api.openaq.org/v3/locations?order_by=id&sort_order=asc&countries_id={countryId}&limit=10&page=1"),
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
            Console.WriteLine($"GetSensorByLocationId: Try get response for {locationId}");
            var response = await _client.SendAsync(request);
            Console.WriteLine($"GetSensorByLocationId: Response code for {locationId} is {response.StatusCode}");

            if (response.StatusCode == (System.Net.HttpStatusCode)429)
                {
                    Console.WriteLine($"GetSensorByLocationId: 429 Try get response for {locationId}");
                    await Task.Delay(10000);
                    Console.WriteLine($"GetSensorByLocationId: Retrying {locationId}");
                    return await GetSensorsByLocationIdAsync(locationId);
                }
            if(response.StatusCode==(System.Net.HttpStatusCode)500)
            {
                Console.WriteLine($"Internal server error for {locationId}");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            GetSensorsByLocationIdResponse getSensorsByLocationIdResponse = JsonConvert.DeserializeObject<GetSensorsByLocationIdResponse>(body);
            return getSensorsByLocationIdResponse;
            
        }

        public async Task<List<GetViewDataResponse>> GetViewDataAsync(int countryId)
        {
            var locations = await GetLocationsAsync(countryId);

            List<GetViewDataResponse> dataResponse = new();
            foreach (var location in locations.results)
            {
                await Task.Delay(3000);
                Console.WriteLine($"GetViewData: Get sensors by {location.id}");
                var sensors = await GetSensorsByLocationIdAsync(location.id);

                if (sensors != null)
                {
                    dataResponse.Add(new GetViewDataResponse()
                    {
                        id = location.id,
                        name = location.name,
                        locality = location.locality,
                        timezone = location.timezone,
                        country = location.country,
                        owner = location.owner,
                        provider = location.provider,
                        isMobile = location.isMobile,
                        isMonitor = location.isMonitor,
                        instruments = location.instruments,

                        sensors = sensors.results,

                        coordinates = location.coordinates,
                        licenses = location.licenses,
                        bounds = location.bounds,
                        distance = location.distance,
                        datetimeFirst = location.datetimeFirst,
                        datetimeLast = location.datetimeLast,
                    });
                }
            }
            return dataResponse;
        }
    }
}
