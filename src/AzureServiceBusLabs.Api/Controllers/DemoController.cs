#region Imports
using AzureServiceBusLabs.Api.Config;
using AzureServiceBusLabs.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        [HttpPost]
        [Route("send")]
        public IActionResult SendMessage(Message message)
        {
            var options = new JsonSerializerOptions()
            {
                MaxDepth = 0,
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var serializedMessage = JsonSerializer.Serialize(message, options);
            var result = _serviceBusSender.Send(serializedMessage);
            return Ok(result);
        }

        [HttpGet]
        [Route("receive")]
        public IActionResult ReceiveMessage()
        {
            var message = _serviceBusReceiver.Receive();
            return Ok(message);
        }

        #endregion
    }
}
