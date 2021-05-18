#region Imports
using AzureServiceBusLabs.Api.Services;
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

        private readonly IServiceBusSender _serviceBusSender;
        private readonly IServiceBusReceiver _serviceBusReceiver;
        private readonly ILogger<DemoController> _logger;

        #endregion

        #region Ctor

        public DemoController(IServiceBusSender serviceBusSender, IServiceBusReceiver serviceBusReceiver, ILogger<DemoController> logger)
        {
            _serviceBusSender = serviceBusSender;
            _serviceBusReceiver = serviceBusReceiver;
            _logger = logger;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult SendMessage([FromQuery] string message)
        {
            var result = _serviceBusSender.Send(message);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult ReceiveMessage()
        {
            var message = _serviceBusReceiver.Receive();
            return Ok(message);
        }

        #endregion
    }
}
