using System;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;

namespace Worker2
{
    internal class Program
    {

        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string MessageName = "receive-offer-response-event-message";

        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a offer response.");
                return;
            }

            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            await client
                .NewPublishMessageCommand()
                .MessageName(MessageName)
                .CorrelationKey(args[0])
                .Variables("{\"signingCompleted\":\"" + args[0] + "\"}")
                .Send();

            Console.WriteLine("Publish offer response event message with correlation id: " + args[0]);
        }
    }
}
