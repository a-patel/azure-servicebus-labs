#region Imports
using Azure.Messaging.ServiceBus;
using AzureServiceBusLabs.Api.Config;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

namespace AzureServiceBusLabs.Api.Services
{
    public class AzureServiceBusSender : IServiceBusSender
    {
        #region Members

        private readonly AzureServiceBusConfig _serviceBusConfig;

        #endregion

        #region Ctor

        public AzureServiceBusSender(IOptions<AzureServiceBusConfig> options)
        {
            _serviceBusConfig = options.Value;
        }

        #endregion

        #region Methods

        public async Task<bool> Send(string messageContent)
        {
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionsString);
            //await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionsString, new Azure.Identity.DefaultAzureCredential());

            // create the sender
            var sender = client.CreateSender(_serviceBusConfig.QueueName);

            // create a message that we can send. UTF-8 encoding is used when providing a string.
            var message = new ServiceBusMessage(messageContent);

            // send the message
            await sender.SendMessageAsync(message);

            Console.WriteLine($"Sent a single message to the queue: {_serviceBusConfig.QueueName}");

            return true;

            //// create a receiver that we can use to receive the message
            //ServiceBusReceiver receiver = client.CreateReceiver(_serviceBusConfig.QueueName);

            //// the received message is a different type as it contains some service set properties
            //ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

            //// get the message body as a string
            //string body = receivedMessage.Body.ToString();
            //Console.WriteLine(body);
        }

        public async Task<bool> Send<T>(T data)
        {
            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionsString);

            // create the sender
            var sender = client.CreateSender(_serviceBusConfig.QueueName);

            var serializedMessage = JsonSerializer.Serialize(data);
            // create a message that we can send. UTF-8 encoding is used when providing a string.
            var message = new ServiceBusMessage(serializedMessage);

            // send the message
            await sender.SendMessageAsync(message);

            Console.WriteLine($"Sent a message to the queue: {_serviceBusConfig.QueueName}");

            return true;
        }

        #endregion
    }
}
