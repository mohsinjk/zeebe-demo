using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zeebe.Client;

namespace api1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZeebeController : ControllerBase
    {
        private readonly ILogger<ZeebeController> _logger;

        private static readonly string ZeebeUrl = "localhost:26500";
        private static readonly string MessageName = "receive-signing-event-message";

        public ZeebeController(ILogger<ZeebeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ZeebeRequest request)
        {
            var client = ZeebeClient.Builder()
               .UseGatewayAddress(ZeebeUrl)
               .UsePlainText()
               .Build();

            await client
                .NewPublishMessageCommand()
                .MessageName(MessageName)
                .CorrelationKey(request.CorrelationKey)
                .Variables("{\"signingCompleted\":true}")
                .Send();

            return Ok();
        }
    }

    public class ZeebeRequest
    {
        public string CorrelationKey { get; set; }
    }
}
