using Shared.Models.Map;

namespace Server.Services
{
    public interface IDataService
    {
        public Task<Country> GetDataFromDbByCountryIdAsync(int countryId);
        public Task PutDataToDbAsync(int countryId);
    }
}
