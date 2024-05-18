using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Data;
using Server.Services;
using Shared.Models.Auth.Requests;
using Shared.Models.OpenAq.Locations;
using System.Net.Http.Headers;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAqController : Controller
    {
        private readonly AppDbContext _dataContext;
        private readonly IOpenAqService _openAqService;

        public OpenAqController(AppDbContext dataContext, IOpenAqService openAqService)
        {
            _dataContext = dataContext;
            _openAqService = openAqService;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _openAqService.GetCountriesAsync();

                return Ok(countries.results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetLocations(int countryId)
        {
            try
            {
                var locations = await _openAqService.GetLocationsAsync(countryId);
                
                return Ok(locations.results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{locationId}")]
        public async Task<IActionResult> GetSensors(int locationId)
        {
            try
            {
                var sensors = await _openAqService.GetSensorsByLocationIdAsync(locationId);
                
                return Ok(sensors.results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
