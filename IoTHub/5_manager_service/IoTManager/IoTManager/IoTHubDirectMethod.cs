using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web.Http;
using Microsoft.Azure.Devices;

namespace IoTManager
{
    public static class IoTHubDirectMethod
    {
        [FunctionName("dm")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("IoTHubDirectMethod API call.");

            var iothubConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnetion");
            var serviceClient = ServiceClient.CreateFromConnectionString(iothubConnectionString);

            var methodInvocation = new CloudToDeviceMethod("opneDoor")
            {
                ResponseTimeout = TimeSpan.FromSeconds(30),
                ConnectionTimeout = TimeSpan.FromSeconds(30),
            };
            methodInvocation.SetPayloadJson("{\"when\":\"now\"}");

            // Direct Method 호출
            try
            {
                var response = await serviceClient.InvokeDeviceMethodAsync(Environment.GetEnvironmentVariable("DeviceId"), methodInvocation);
                if (response.Status == 200)
                {
                    return new OkObjectResult(response.GetPayloadAsJson());
                }
                else
                {
                    return new BadRequestErrorMessageResult(response.GetPayloadAsJson());
                }
            }
            catch (Exception ex)
            {
                // Exception 처리 필수
                log.LogError(ex.ToString());
                return new InternalServerErrorResult();
            }
        }
    }
}
