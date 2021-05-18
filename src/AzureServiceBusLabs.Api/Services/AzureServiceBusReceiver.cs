#region Imports
using Azure.Messaging.ServiceBus;
using AzureServiceBusLabs.Api.Config;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureServiceBusLabs.Api.Services
{
    public class AzureServiceBusReceiver : IServiceBusReceiver
    {
        #region Members

        private readonly AzureServiceBusConfig _serviceBusConfig;

        #endregion

        #region Ctor

        public AzureServiceBusReceiver(IOptions<AzureServiceBusConfig> options)
        {
            _serviceBusConfig = options.Value;
        }

        #endregion

        #region Methods

        public async Task<string> Receive()
        {
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionsString);

            // create a receiver that we can use to receive the message
            var receiver = client.CreateReceiver(_serviceBusConfig.QueueName);

            // the received message is a different type as it contains some service set properties
            var receivedMessage = await receiver.ReceiveMessageAsync();

            // get the message body as a string
            var messageContent = receivedMessage.Body.ToString();

            return messageContent;
        }

        #endregion
    }
}
