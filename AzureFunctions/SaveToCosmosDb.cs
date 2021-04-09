using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class SaveToCosmosDb
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToCosmosDb")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection", ConsumerGroup = "cosmosdb")]EventData message, 
            [CosmosDB(
                databaseName:"iot20ahmedcosmosdb", 
                collectionName:"Messages", 
                ConnectionStringSetting ="CosmosDbConnection",
                CreateIfNotExists = true
            )]out dynamic cosmos,
            ILogger log)
        {
            log.LogInformation($"Incomming message: {Encoding.UTF8.GetString(message.Body.Array)}");

            cosmos = Encoding.UTF8.GetString(message.Body.Array);
        }
    }
}