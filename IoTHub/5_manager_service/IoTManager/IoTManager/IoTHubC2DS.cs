using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Web.Http;
using System.Text;

namespace IoTManager
{
    public static class IoTHubC2DS
    {
        public static object ExceptionHelper { get; private set; }

        [FunctionName("c2d")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("c3d api callled.");

            var iothubConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnetion");
            var serviceClient = ServiceClient.CreateFromConnectionString(iothubConnectionString);

            var message = new Message(Encoding.ASCII.GetBytes("{ \"c2d\": \"message\"}"))
            {
                // An acknowledgment is sent on delivery success or failure.
                Ack = DeliveryAcknowledgement.Full
            };

            try
            {
                await serviceClient.SendAsync(Environment.GetEnvironmentVariable("DeviceId"), message, TimeSpan.FromSeconds(10));
                log.LogInformation($"Sent c2d message {message}");
                message.Dispose();

                return new OkObjectResult("");
            }
            catch (Exception e)
            {
                log.LogError($"Unexpected error, will need to reinitialize the client: {e}");
                return new BadRequestErrorMessageResult(e.ToString());
            }

            return new OkObjectResult("");
        }
    }
}
