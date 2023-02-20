using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ServiceBusScheduler;

public class ProcessMessage
{
    [FunctionName("ProcessMessage")]
    public void Run([ServiceBusTrigger("<queue-name>")] string itemFromQueue, ILogger log)
    {
        log.LogInformation("no-ghost: Processing message from queue.");
        var message = JObject.Parse(itemFromQueue);

        var keyValueString = string.Join(", ", message.Properties().Select(p => $"{p.Name}: {p.Value}"));
        log.LogInformation($"no-ghost: Successfully processed message and pulled the following data: {keyValueString}");
        // Continue logic with message data
    }
}