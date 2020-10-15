using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubsReceiver
{
    class Program
    {
        private const string ehubNamespaceConnectionString = "";
        private const string eventHubName = "myeventhub";

        static async Task Main()
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            var receiverClient = new EventHubConsumerClient(ehubNamespaceConnectionString, eventHubName);

            EventProcessorClient processor = new EventProcessorClient();

            var info = await receiverClient.GetPartitionIdsAsync();

            Console.WriteLine(info);
            //await receiverClient.StartProcessingAsync();

            

            //await processor.StopProcessingAsync();

        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine($"\tPartition '{arg.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(arg.Exception.Message);
            return Task.CompletedTask;
        }

        static async Task GetPartitionIdsAsync(ProcessEventArgs arg)
        {
            Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(arg.Data.Body.ToArray()));

            await arg.UpdateCheckpointAsync(arg.CancellationToken);
        }
    }
}
