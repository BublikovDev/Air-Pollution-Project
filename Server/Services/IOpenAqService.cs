using Shared.Models.OpenAq.Countries;
using Shared.Models.OpenAq.Locations;
using Shared.Models.OpenAq.Sensors;
using Shared.Models.OpenAq.ViewData;

namespace Server.Services
{
    public interface IOpenAqService
    {
        public Task<GetCountriesResponse> GetCountriesAsync();
        public Task<GetLocationsResponse> GetLocationsAsync(int countryId);
        public Task<GetSensorsByLocationIdResponse> GetSensorsByLocationIdAsync(int locationId);
        public Task<List<GetViewDataResponse>> GetViewDataAsync(int countryId);
    }
}
