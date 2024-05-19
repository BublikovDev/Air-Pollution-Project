
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models.Map;

namespace Server.Services
{
    public class DataService : IDataService
    {
        private readonly IOpenAqService _openAqService;
        private readonly AppDbContext _appDbContext;

        public DataService(IOpenAqService openAqService, AppDbContext appDbContext)
        {
            _openAqService = openAqService;
            _appDbContext = appDbContext;
        }

        public async Task<Country> GetDataFromDbByCountryIdAsync(int countryId)
        {
            var data = await _appDbContext.Countries
                .Include(c => c.Locations)
                .ThenInclude(l=>l.Sensors)
                .FirstOrDefaultAsync(c=>c.Id==countryId);


            return data;
        }

        public async Task PutDataToDbAsync(int countryId)
        {
            var data = await _openAqService.GetViewDataAsync(countryId);
            var country = await _appDbContext.Countries.FindAsync(countryId);
            if (country == null)
            {
                country = new Country()
                {
                    Id = countryId,
                    Code = data.FirstOrDefault().country.code,
                    Name = data.FirstOrDefault().country.name,

                    Locations = new List<Location>()
                };
                await _appDbContext.Countries.AddAsync(country);
            }
            foreach (var item in data)
            {
                var location = await _appDbContext.Locations.FindAsync(item.id);
                if(location == null)
                {
                    location = new Location()
                    {
                        Id = item.id,
                        Name = item.name,
                        Latitude=item.coordinates.latitude,
                        Longitude=item.coordinates.longitude,

                        Sensors=new List<Sensor>(),
                        
                        Country= country,
                        CountryId = countryId,
                    };
                    country.Locations.Add(location);
                    await _appDbContext.Locations.AddAsync(location);
                }
                else
                {
                    location.Name= item.name;
                    location.Latitude=item.coordinates.latitude;
                    location.Longitude=item.coordinates.longitude;
                }
                foreach(var sensorItem in item.sensors)
                {
                    var sensor = await _appDbContext.Sensors.FindAsync(sensorItem.id);
                    if (sensor == null)
                    {
                        sensor = new Sensor()
                        {
                            Id=sensorItem.id,
                            Name = sensorItem.name,
                            LatestTimeUtc=sensorItem.latest.datetime.utc,
                            LatestTimeLocal=sensorItem.latest.datetime.local,
                            Value=sensorItem.latest.value,
                            MinValue=sensorItem.summary.min,
                            MaxValue=sensorItem.summary.max,
                            AvgValue=sensorItem.summary.avg,

                            LocationId=location.Id,
                            Location=location,
                        };
                        
                        await _appDbContext.Sensors.AddAsync(sensor);
                        
                    }
                    else
                    {
                        sensor.Name= sensorItem.name;
                        sensor.LatestTimeLocal=sensorItem.latest.datetime.local;
                        sensor.LatestTimeUtc=sensorItem.latest.datetime.utc;
                        sensor.Value=sensorItem.latest.value;
                        sensor.MinValue=sensorItem.summary.min;
                        sensor.MaxValue=sensorItem.summary.max;
                        sensor.AvgValue=sensorItem.summary.avg;
                    }
                    location.Sensors.Add(sensor);
                }
                
            }
            
            await _appDbContext.SaveChangesAsync();
        }
    }
}
