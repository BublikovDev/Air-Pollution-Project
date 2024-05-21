using Shared.Models.Map;

namespace Server.Services
{
    public interface ICountryService
    {
        public Task CreateAsync(Country country);
        public Task<List<Country>> ReadCountriesAsync();
        public Task<Country> ReadCountryAsync(int countryId);
        public Task UpdateCountryAsync(Country country);
        public Task DeleteCountryAsync(int countryId);
        public Task RecoverCountryAsync(int countryId);
    }
}
