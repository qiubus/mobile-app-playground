using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
                if (disco.IsError)
                {
                    //(disco.Error);
                    // throw new Exception("Identity server unavailable");
                }

                // request token
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "firstClient",
                    ClientSecret = "mySecret",
                    Scope = "workerAppApi"
                });

                if (tokenResponse.IsError)
                {
                    //Console.WriteLine(tokenResponse.Error);
                    throw new Exception("Cannot login user");
                }

                Console.WriteLine(tokenResponse.Json);

                // TODO: check tokenResponse.Json

                var content = tokenResponse.Json.ToString();

                return new List<WeatherForecast>();

            }
        }
    }
}
