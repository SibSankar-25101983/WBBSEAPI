using Microsoft.AspNetCore.Mvc;
using WebApiDevelopment.Models;
using WebApiDevelopment.ViewModel;


namespace WebApiDevelopment.Controllers
{
    [ApiController]
    //[Route("[controller]/[action]")]
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

        [HttpGet("GetWeatherForecast")]
       // [Route("WeatherForecast/GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Test")]
       //[Route("WeatherForecast/Test")]
        public IActionResult Test()
        {
            return Ok("Hello");

        }


        [HttpGet("GetDistrictList")]
        public IActionResult GetDistrictList()
        {
            var Result=new MDistricts().getMstDistrictList();
            
            return Ok(Result);

        }


        [HttpPost("SaveDistrict")]
        public IActionResult SaveDistrict(VMMstDistrict data)
        {
            var Result = new MDistricts().saveMstDistrict(data);

            return Ok(Result);

        }


        [HttpPut("UpdateDistrict")]
        public IActionResult UpdateDistrict(VMMstDistrict data)
        {
            var Result = new MDistricts().UpdateDistrict(data);

            return Ok(Result);

        }

        [HttpDelete("DeleteDistrict")]
        public IActionResult DeleteDistrict(VMMstDistrict data)
        {
            var Result = new MDistricts().DeleteDistrict(data);

            return Ok(Result);

        }
    }
}
