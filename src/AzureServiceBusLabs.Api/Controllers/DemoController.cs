#region Imports
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
#endregion

namespace AzureServiceBusLabs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        #region Members

        private readonly ILogger<DemoController> _logger;

        #endregion

        #region Ctor

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult SendMessage([FromQuery] string message)
        {
            return Ok();
        }

        #endregion
    }
}
