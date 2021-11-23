using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IoTTriggerFunction
{
    public class IoTTriggerFunction
    {
        private static HttpClient client = new HttpClient();
        
        [FunctionName("IoTTriggerFunction")]
        public async Task Run([IoTHubTrigger("messages/events", Connection = "IoTHubBuiltInEndporint")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");

            // 1. Get Message 
            var msgbody = Encoding.UTF8.GetString(message.Body.Array);

            // 2. Get Properties 
            // System Properties Name: https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-messages-construct
            var tempAlert = bool.Parse(message.Properties["temperatureAlert"].ToString());
            var deviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
            var iotHubEnqueueTime = DateTimeOffset.Parse(message.SystemProperties["iothub-enqueuedtime"].ToString());

            log.LogInformation($"message from {deviceId} at {iotHubEnqueueTime} with {tempAlert} temperature alert");

            // 3. Deserialize 
            var telemetry = JsonConvert.DeserializeObject<TelemetryMessage>(msgbody);

            log.LogInformation($"temperature: {telemetry.temperature}, humidity: {telemetry.humidity}");

            // 4. Data manipulation 

            // 4. Save Data 
            // SQL Database sample(Microsoft.Data.SqlClient): https://docs.microsoft.com/ko-kr/azure/azure-functions/functions-scenario-database-table-cleanup
            // Get the connection string from app settings
            var connectionString = Environment.GetEnvironmentVariable("SQLDatabaseConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var sql = $"INSERT INTO [dbo].[Table] VALUES ({telemetry.temperature}, {telemetry.humidity}, '{iotHubEnqueueTime}')"; 
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were inserted.");
                }
            }

        }
    }
}