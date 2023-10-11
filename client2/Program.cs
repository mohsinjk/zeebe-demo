using System;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;

namespace Client1
{
    internal class Program
    {
        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string ProcessId = "privateloan-broker-process";

        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric value as parameter.");
                return;
            }

            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            string variables = "{\"customerId\": \"" + args[0] + "\",\"loanAmount\": " + args[1] + ",\"customerName\": \"ABC XYZ\"}";

            Console.WriteLine($"Starting workflow with id: " + args[0]);

            await client
                .NewCreateProcessInstanceCommand()
                .BpmnProcessId(ProcessId)
                .LatestVersion()
                .Variables(variables)
                .Send();
        }
    }
}
