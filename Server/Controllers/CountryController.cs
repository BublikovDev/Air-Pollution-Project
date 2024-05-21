using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models.Map;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            try
            {
                await _countryService.CreateAsync(country);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _countryService.ReadCountriesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetById(int countryId)
        {
            try
            {
                var response = await _countryService.ReadCountryAsync(countryId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPut("[action]/{countryId}")]
        public async Task<IActionResult> Update(Country country) 
        {
            try
            {
                await _countryService.UpdateCountryAsync(country);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
