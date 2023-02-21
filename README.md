# Service Bus Scheduler
An Azure Function App that schedules messages to be enqueued in a Azure Service Bus Queue and reads the messages once they are enqueued.

## Overview
The Azure Function App consists of two functions: `ScheduleMessage` and `ReadMessage`.  
### `ScheduleMessage`
The `ScheduleMessage` function has an HTTP trigger that takes a body and an `interval` query param.  
It creates a `ServiceBusMessage` out of the body and sets `message.ScheduledEnqueueTime` to `interval` minutes from now before publishing it to a queue.  
### `ReadMessage`
The `ReadMessage` function has a `ServiceBus` trigger on the queue `ScheduleMessage` publishes to, so once the message is enqueued, it pulls the messages off and logs it.  
Where `ReadMessage` logs the message would be where additional logic that needed to occur at the scheduled time would be implemented. 

## Running the app
1. Create a Service Bus Namespace and a Queue in the Azure Portal.
2. Create a `AzureWebJobsServiceBus` key with a value of the connection string of the Service Bus Namespace in the `local.settings.json` file.
3. Paste the name of the Service Bus Queue where `<queue-name>` is used in the `ScheduleMessage` and `ReadMessage` functions.
4. Build and run the app using Visual Studio or Docker.
5. Send a HTTP request to the `ScheduleMessage` function using a tool like Postman with a body and an `interval` query parameter.  
The Service Bus message will be scheduled for `interval` minutes from now.  
The body can contain any properties. For example:
```curl
curl --location 'http://localhost:7071/api/ScheduleMessage?interval=1' \
--header 'Content-Type: application/json' \
--data '{
    "message": "Service Bus Scheduler"
}'
```
6. Once the interval has passed and the message is enqueued, the `ReadMessage` function will pick the message off of the queue and log the key/value pairs of the object sent as the body.
