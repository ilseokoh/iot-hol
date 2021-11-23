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

namespace IoTManager
{
    public static class IoTHubDeviceTwin
    {
        [FunctionName("twin")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("device twin called."); 
            
            var iothubConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnetion");
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(iothubConnectionString);

            var twin = await registryManager.GetTwinAsync(Environment.GetEnvironmentVariable("DeviceId"));

            Random random = new Random();
            var patch = $"{{\"firmware\": \"1.2.3\",\"fanSpeed\": {random.Next()}}}";
            await registryManager.UpdateTwinAsync(Environment.GetEnvironmentVariable("DeviceId"), patch, twin.ETag);
            // twin을 조회해서 Etag를 가져와서 바로 호출

            return new OkObjectResult("");
        }
    }
}
