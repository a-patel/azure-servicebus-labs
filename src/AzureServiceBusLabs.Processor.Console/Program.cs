#region Imports
using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureServiceBusLabs.Processor.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var _serviceBusConfig = new
            {
                ConnectionsString = "",
                QueueName = ""
            };

            // create the options to use for configuring the processor
            var options = new ServiceBusProcessorOptions
            {
                // By default or when AutoCompleteMessages is set to true, the processor will complete the message after executing the message handler
                // Set AutoCompleteMessages to false to [settle messages](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock) on your own.
                // In both cases, if the message handler throws an exception without settling the message, the processor will abandon the message.
                AutoCompleteMessages = false,

                // I can also allow for multi-threading
                MaxConcurrentCalls = 2
            };

            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionsString);

            // create a processor that we can use to process the messages
            await using ServiceBusProcessor processor = client.CreateProcessor(_serviceBusConfig.QueueName, options);

            // configure the message and error handler to use
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            async Task MessageHandler(ProcessMessageEventArgs args)
            {
                string body = args.Message.Body.ToString();
                System.Console.WriteLine(body);

                // we can evaluate application logic and use that to determine how to settle the message.
                await args.CompleteMessageAsync(args.Message);
            }

            Task ErrorHandler(ProcessErrorEventArgs args)
            {
                // the error source tells me at what point in the processing an error occurred
                System.Console.WriteLine(args.ErrorSource);
                // the fully qualified namespace is available
                System.Console.WriteLine(args.FullyQualifiedNamespace);
                // as well as the entity path
                System.Console.WriteLine(args.EntityPath);
                System.Console.WriteLine(args.Exception.ToString());
                return Task.CompletedTask;
            }

            // start processing
            await processor.StartProcessingAsync();

            // since the processing happens in the background, we add a Conole.ReadKey to allow the processing to continue until a key is pressed.
            System.Console.ReadKey();
        }
    }
}
