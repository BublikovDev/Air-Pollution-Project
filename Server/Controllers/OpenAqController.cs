using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Data;
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

        [AllowAnonymous]
        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetLocations(int countryId)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.openaq.org/v3/locations?order_by=id&sort_order=asc&countries_id={countryId}&limit=100&page=1"),
                    Headers =
                    {
                        { "accept", "application/json" },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    GetLocationsResponse myDeserializedClass = JsonConvert.DeserializeObject<GetLocationsResponse>(body);
                    return Ok(myDeserializedClass);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
