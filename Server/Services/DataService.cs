
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
                .ThenInclude(l => l.Sensors)
                .FirstOrDefaultAsync(c => c.Id == countryId);


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
                if (location == null)
                {
                    location = new Location()
                    {
                        Id = item.id,
                        Name = item.name,
                        Latitude = item.coordinates.latitude,
                        Longitude = item.coordinates.longitude,

                        Sensors = new List<Sensor>(),

                        Country = country,
                        CountryId = countryId,
                    };
                    country.Locations.Add(location);
                    await _appDbContext.Locations.AddAsync(location);
                }
                else
                {
                    location.Name = item.name;
                    location.Latitude = item.coordinates.latitude;
                    location.Longitude = item.coordinates.longitude;
                }
                var AQI = 0.0;
                foreach (var sensorItem in item.sensors)
                {
                   
                    var subAqi = CalculateSubIndex(sensorItem.name, sensorItem.latest.value);
                    if (subAqi > AQI) { AQI = subAqi; }
                    var sensorDb = await _appDbContext.Sensors.FindAsync(sensorItem.id);
                    if (sensorDb == null)
                    {
                        sensorDb = new Sensor()
                        {
                            Id = sensorItem.id,
                            Name = sensorItem.name,
                            LatestTimeUtc = sensorItem.latest.datetime.utc,
                            LatestTimeLocal = sensorItem.latest.datetime.local,
                            Value = sensorItem.latest.value,
                            MinValue = sensorItem.summary.min,
                            MaxValue = sensorItem.summary.max,
                            AvgValue = sensorItem.summary.avg,

                            LocationId = location.Id,
                            Location = location,
                        };
                        
                        await _appDbContext.Sensors.AddAsync(sensorDb);

                    }
                    else
                    {
                        sensorDb.Name = sensorItem.name;
                        sensorDb.LatestTimeLocal = sensorItem.latest.datetime.local;
                        sensorDb.LatestTimeUtc = sensorItem.latest.datetime.utc;
                        sensorDb.Value = sensorItem.latest.value;
                        sensorDb.MinValue = sensorItem.summary.min;
                        sensorDb.MaxValue = sensorItem.summary.max;
                        sensorDb.AvgValue = sensorItem.summary.avg;
                    }
                    location.Sensors.Add(sensorDb);
                    
                }
                var sensorAqi = await _appDbContext.Sensors.Where(s=>s.LocationId==location.Id).FirstOrDefaultAsync(s=>s.Name=="AQI");
                if (sensorAqi == null)
                {
                    sensorAqi = new Sensor()
                    {
                        Id = 14888841,
                        Name = "AQI",
                        LatestTimeUtc = DateTime.UtcNow,
                        LatestTimeLocal = DateTime.Now,
                        Value = AQI,
                        MinValue = AQI,
                        MaxValue = AQI,
                        AvgValue = AQI,

                        LocationId = location.Id,
                        Location = location,
                    };

                    await _appDbContext.Sensors.AddAsync(sensorAqi);
                }
                else
                {
                    sensorAqi.Name = "AQI";
                    sensorAqi.LatestTimeUtc = DateTime.UtcNow;
                    sensorAqi.LatestTimeLocal = DateTime.Now;
                    sensorAqi.Value = AQI;
                    if (sensorAqi.MinValue > AQI) { sensorAqi.MinValue = AQI; }
                    if (sensorAqi.MaxValue < AQI) { sensorAqi.MaxValue = AQI; }
                }
                location.Sensors.Add(sensorAqi);
            }

            await _appDbContext.SaveChangesAsync();
        }


        public static double CalculateSubIndex(string name, double value)
        {
            double subIndex = 0;

            switch (name.ToLower())
            {
                case "pm10":
                    subIndex = CalculatePM10SubIndex(value);
                    break;
                case "pm2.5":
                    subIndex = CalculatePM25SubIndex(value);
                    break;
                case "no2":
                    subIndex = CalculateNO2SubIndex(value);
                    break;
                case "o3":
                    subIndex = CalculateO3SubIndex(value);
                    break;
                case "co":
                    subIndex = CalculateCOSubIndex(value);
                    break;
                case "so2":
                    subIndex = CalculateSO2SubIndex(value);
                    break;
                default:
                    throw new ArgumentException("Unknown parameter name");
            }

            return subIndex;
        }

        private static double CalculatePM10SubIndex(double value)
        {
            if (value <= 25) return Interpolate(value, 0, 25, 0, 25);
            if (value <= 50) return Interpolate(value, 25, 50, 26, 50);
            if (value <= 90) return Interpolate(value, 50, 90, 51, 75);
            if (value <= 180) return Interpolate(value, 90, 180, 76, 100);
            return Interpolate(value, 180, 360, 101, 200);
        }

        private static double CalculatePM25SubIndex(double value)
        {
            if (value <= 15) return Interpolate(value, 0, 15, 0, 25);
            if (value <= 30) return Interpolate(value, 15, 30, 26, 50);
            if (value <= 55) return Interpolate(value, 30, 55, 51, 75);
            if (value <= 110) return Interpolate(value, 55, 110, 76, 100);
            return Interpolate(value, 110, 220, 101, 200);
        }

        private static double CalculateNO2SubIndex(double value)
        {
            if (value <= 50) return Interpolate(value, 0, 50, 0, 25);
            if (value <= 100) return Interpolate(value, 50, 100, 26, 50);
            if (value <= 200) return Interpolate(value, 100, 200, 51, 75);
            if (value <= 400) return Interpolate(value, 200, 400, 76, 100);
            return Interpolate(value, 400, 800, 101, 200);
        }

        private static double CalculateO3SubIndex(double value)
        {
            if (value <= 60) return Interpolate(value, 0, 60, 0, 25);
            if (value <= 120) return Interpolate(value, 60, 120, 26, 50);
            if (value <= 180) return Interpolate(value, 120, 180, 51, 75);
            if (value <= 240) return Interpolate(value, 180, 240, 76, 100);
            return Interpolate(value, 240, 480, 101, 200);
        }

        private static double CalculateCOSubIndex(double value)
        {
            if (value <= 5) return Interpolate(value, 0, 5, 0, 25);
            if (value <= 10) return Interpolate(value, 5, 10, 26, 50);
            if (value <= 20) return Interpolate(value, 10, 20, 51, 75);
            if (value <= 40) return Interpolate(value, 20, 40, 76, 100);
            return Interpolate(value, 40, 80, 101, 200);
        }
        
        private static double CalculateSO2SubIndex(double value)
        {
            if (value <= 50) return Interpolate(value, 0, 50, 0, 25);
            if (value <= 100) return Interpolate(value, 50, 100, 26, 50);
            if (value <= 200) return Interpolate(value, 100, 200, 51, 75);
            if (value <= 350) return Interpolate(value, 200, 350, 76, 100);
            return Interpolate(value, 350, 700, 101, 200);
        }

        private static double Interpolate(double value, double lowValue, double highValue, double lowIndex, double highIndex)
        {
            return (value - lowValue) / (highValue - lowValue) * (highIndex - lowIndex) + lowIndex;
        }

    }
}
