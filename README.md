# Service Bus Scheduler
An Azure Function App that schedules messages to be enqueued in a Azure Service Bus Queue and reads the messages once they are enqueued.

## Overview
The Azure Function App consists of two functions: `ScheduleMessage` and `ReadMessage`.  
The `ScheduleMessage` function is triggered by a HTTP request and enqueues a message in a Service Bus Queue.  
The `ReadMessage` function is triggered by a Service Bus Queue message and writes the message to the console.

## Running the app
1. Create a Service Bus Namespace and a Queue in the Azure Portal.
2. Create a `AzureWebJobsServiceBus` key with a value of the connection string of the Service Bus Namespace in the `local.settings.json` file.
3. Paste the name of the Service Bus Queue where `<queue-name>` is used in the `ScheduleMessage` and `ReadMessage` functions.
4. Build and run the app using Visual Studio or Docker.
5. Send a HTTP request to the `ScheduleMessage` function using a tool like Postman with a body an `interval` query parameter with a value of the number of minutes to wait before the message is enqueued.  
The body can contain any properties. For example:
```curl
curl --location 'http://localhost:7071/api/ScheduleMessage?interval=1' \
--header 'Content-Type: application/json' \
--data '{
    "message": "Service Bus Scheduler"
}'
```
6. The `ReadMessage` function will write the message to the console once it is enqueued.
