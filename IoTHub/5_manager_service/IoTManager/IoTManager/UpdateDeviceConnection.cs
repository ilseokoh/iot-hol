using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;

namespace IoTManager
{
    public static class UpdateDeviceConnection
    {
        [FunctionName("UpdateDeviceConnection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("IoT Hub Device connection status.");
            BinaryData events = await BinaryData.FromStreamAsync(req.Body);

            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        log.LogInformation($"Got SubscriptionValidation event data, validation code: {subscriptionValidationEventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                        // Do any additional validation (as required) and then return back the below response

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };
                        return new OkObjectResult(responseData);
                    }
                }

                var data = eventGridEvent.Data.ToString();
                // "data": {
                //      "deviceConnectionStateEventInfo": {
                //         "sequenceNumber": "000000000000000001D4132452F67CE200000002000000000000000000000001"
                //       },
                // "hubName": "townbroadcast-hub",
                // "deviceId": "LogicAppTestDevice"
                // }

                log.LogInformation($"eventGridEvent.Data: {eventGridEvent.Data.ToString()}");
                var eventdata = JsonConvert.DeserializeObject<EventGridData>(data);

                log.LogInformation($"{eventdata.deviceId} is {(eventGridEvent.EventType == "Microsoft.Devices.DeviceConnected" ? "Connected" : "Disconnected")}");

                switch (eventGridEvent.EventType)
                {
                    case "Microsoft.Devices.DeviceDisconnected":
                        break;
                    case "Microsoft.Devices.DeviceConnected":
                        break;

                }
            }

            return new OkObjectResult("");
        }
    }
}
