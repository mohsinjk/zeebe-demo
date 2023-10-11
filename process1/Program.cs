using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;

namespace process1
{
    internal class Program
    {
        private const string Process = "loan-disbursement-process";
        private static readonly string ZeebeUrl = "localhost:26500";

        public static async Task Main(string[] args)
        {
            try
            {
                await DeployProcess();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

        }

        public static async Task DeployProcess()
        {
            // create zeebe client
            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            Console.WriteLine($"Deploying {Process}", Process);
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                var processPath = Path.Combine(
                    Path.GetDirectoryName(typeof(Program).Assembly.Location),
                    $"./{Process}.bpmn");

                await client
                    .NewDeployCommand()
                    .AddResourceFile(processPath)
                    .Send();

                var deployResponse = await client.NewDeployCommand()
               .AddResourceFile(processPath)
               .Send();

                Console.WriteLine($"Deployed {Process}", Process);
            }
        }

    }
}
