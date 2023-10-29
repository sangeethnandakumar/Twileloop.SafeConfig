using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Template_CongurationValidationWithFluent.Validators;

namespace Template_CongurationValidationWithFluent.Controllers
{
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IOptions<SMTPOptions> options;

        public DemoController(IOptions<SMTPOptions> options)
        {
            this.options = options;
        }

        [HttpGet]
        [Route("Demo")]
        public IEnumerable<WeatherForecast> Get()
        {
            var data = options.Value;

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
            })
            .ToArray();
        }
    }
}
