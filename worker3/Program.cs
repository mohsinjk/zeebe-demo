using System;
using System.Threading;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Worker5
{
    internal class Program
    {
        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string JobType = "create-digital-signing";
        private static readonly string WorkerName = Environment.MachineName;

        public static void Main(string[] args)
        {

            var client = ZeebeClient.Builder()
                .UseLoggerFactory(new NLogLoggerFactory())
                .UseGatewayAddress(ZeebeUrl)
                .UsePlainText()
                .Build();

            // open job worker
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                client.NewWorker()
                      .JobType(JobType)
                      .Handler(HandleJob)
                      .MaxJobsActive(5)
                      .Name(WorkerName)
                      .PollInterval(TimeSpan.FromSeconds(1))
                      .Timeout(TimeSpan.FromMinutes(10))
                      .Open();

                Console.WriteLine("Worker 3 with job type '{0}' is running.", JobType);

                // blocks main thread, so that worker can run
                signal.WaitOne();
            }
        }

        public static async Task HandleJob(IJobClient jobClient, IJob job)
        {
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);
            // business logic
            Console.WriteLine("Worker 3 completes job successfully.");

            jobClient.NewCompleteJobCommand(jobKey)
                .Variables("{\"signingOrderId\":123456789}")
                .Send()
                .GetAwaiter()
                .GetResult();

        }
    }
}
