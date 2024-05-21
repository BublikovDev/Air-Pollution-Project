using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.Map;

namespace Server.Services
{
    public class CountryService : ICountryService
    {
        private readonly AppDbContext _appDbContext;

        public CountryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateAsync(Country country)
        {
            await _appDbContext.Countries.AddAsync(country);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Country>> ReadCountriesAsync()
        {
            var data = await _appDbContext.Countries.ToListAsync();
            if(data==null)
                throw new Exception("Countries is not found");

            return data;
        }

        public async Task<Country> ReadCountryAsync(int countryId)
        {
            var countryDb = await _appDbContext.Countries.FindAsync(countryId);
            if (countryDb == null)
                throw new Exception("Country is not found");
            return countryDb;
        }

        public async Task UpdateCountryAsync(Country country)
        {
            var countryDb = await _appDbContext.Countries.FindAsync(country.Id);
            if (countryDb == null)
                throw new Exception("Country is not found");
            countryDb.Name=country.Name;
            countryDb.Code=country.Code;

            await _appDbContext.SaveChangesAsync();
        }
        
        public async Task DeleteCountryAsync(int countryId)
        {
            var countryDb = await _appDbContext.Countries.FindAsync(countryId);
            if (countryDb == null)
                throw new Exception("Country is not found");
            countryDb.IsDeleted = false;
        }

        public async Task RecoverCountryAsync(int countryId)
        {
            var countryDb = await _appDbContext.Countries.FindAsync(countryId);
            if (countryDb == null)
                throw new Exception("Country is not found");
            countryDb.IsDeleted = true;
        }

        
    }
}
