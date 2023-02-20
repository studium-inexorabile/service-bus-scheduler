using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ServiceBusScheduler;

public static class ScheduleMessage
{
    [FunctionName("ScheduleMessage")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [ServiceBus("<queue-name>")] IAsyncCollector<ServiceBusMessage> outputMessages,
        ILogger log)
    {
        log.LogInformation("no-ghost: Processing message for scheduled enqueue");
        string body;
        using (var reader = new StreamReader(req.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
            log.LogInformation($"Message body : {body}");
        }

        var interval = 5;
        if (int.TryParse(req.Query["interval"], out var parsedInterval))
        {
            interval = parsedInterval;
        }

        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(body))
        {
            ScheduledEnqueueTime = DateTime.UtcNow.AddMinutes(interval)
        };

        await outputMessages.AddAsync(message);

        var successMessage = $"Message scheduled for enqueue in {interval} minutes ({message.ScheduledEnqueueTime})";
            
        log.LogInformation("no-ghost: " + successMessage);
        return new OkObjectResult(successMessage);
    }
}