using System;
using System.Threading;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Newtonsoft.Json;

namespace Worker1
{
    internal class Application
    {
        public int CustomerId { get; set; }
        public int LoanAmount { get; set; }
    }
    internal class Program
    {
        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string JobType = "create-loan-application";
        private static readonly string WorkerName = Environment.MachineName;
        private static bool test = false;
        public static Task Main(string[] args)
        {
            if (Environment.GetEnvironmentVariable("ZEEBE_WORKER_MODE") != null)
                test = Environment.GetEnvironmentVariable("ZEEBE_WORKER_MODE") == "test";
            else
                if (args.Length == 0)
            {
                Console.WriteLine("Enter a parameter (test or normal) or ...");
                Console.WriteLine("Set the environment variable ZEEBE_WORKER_MODE to either 'test' or 'normal'.");
                return Task.CompletedTask;
            }
            else
                test = args[0] == "test";

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

                Console.WriteLine("Worker 1 with job type '{0}' is running in {1} mode.", JobType, test ? "test" : "normal");

                // blocks main thread, so that worker can run
                signal.WaitOne();
            }

            return Task.CompletedTask;
        }

        private static void HandleJob(IJobClient jobClient, IJob job)
        {
            // business logic
            var jobKey = job.Key;
            Console.WriteLine("Worker 1 handling job: " + job);
            var application = JsonConvert.DeserializeObject<Application>(job.Variables);


            if (!test)
            {
                Console.WriteLine("Worker 1 completes job successfully.");
                jobClient.NewCompleteJobCommand(jobKey)
                    .Variables("{\"applicationId\":\"" + Guid.NewGuid() + "\"}")
                    .Send()
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                Console.WriteLine("Worker failing with message: {0}", "Backend system not available");
                jobClient.NewFailCommand(jobKey)
                    .Retries(job.Retries - 1)
                    .ErrorMessage("Backend system not available.")
                    .Send()
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
