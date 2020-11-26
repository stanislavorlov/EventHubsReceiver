using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EHReaderConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ListenEvenHub().Wait();
        }

        static async Task ListenEvenHub()
        {
            var connectionString = "";
            var eventHubName = "";
            var consumerGroup = "";

            await using (var client = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName))
            {
                using var cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

                await foreach (PartitionEvent partitionEvent in client.ReadEventsAsync(cancellationSource.Token))
                {
                    Console.WriteLine(Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray()));
                }
            }
        }
    }
}
