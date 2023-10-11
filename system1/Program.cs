using System;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;

namespace Worker2
{
    internal class Program
    {

        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string MessageName = "receive-signing-event-message";

        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument.");
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
                .Variables("{\"signingCompleted\":true}")
                .Send();

            Console.WriteLine("Publish signing event message with correlation id: " + args[0]);
        }
    }
}
