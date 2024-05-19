using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetData(int countryId)
        {
            try
            {
                var data = await _dataService.GetDataFromDbByCountryIdAsync(countryId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPut("[action]/{countryId}")]
        public async Task<IActionResult> PutData(int countryId)
        {
            try
            {
                await _dataService.PutDataToDbAsync(countryId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
