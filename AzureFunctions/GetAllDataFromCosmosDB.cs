using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class GetAllDataFromCosmosDB
    {
        [FunctionName("GetAllDataFromCosmosDB")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName:"iot20ahmedcosmosdb",
                collectionName:"Messages",
                ConnectionStringSetting ="CosmosDbConnection",
                SqlQuery = "SELECT * FROM c"
            )]IEnumerable<dynamic>cosmos,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(cosmos);
        }
    }
}

