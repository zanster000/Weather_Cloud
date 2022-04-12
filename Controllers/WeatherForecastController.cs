using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Weather_Cloud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HttpClient _httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this._httpClient = httpClientFactory.CreateClient("hg");
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var path = "weather";

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(responseJson);
        }
    }
}
