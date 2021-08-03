using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using productsAPIs.Models;

namespace productsAPIs
{
    public static class GetRatings
    {
         [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("ratings")] CloudTable ratingTable,
            ILogger log)
        {
            log.LogInformation("GetRating HTTP trigger function processed a request.");

            string userId = req.Query["userId"];

            TableOperation tableOperation =
                TableOperation.Retrieve<RatingResponse>("RATING", userId);
            var result = await ratingTable.ExecuteAsync(tableOperation);
            var rating = result.Result as RatingResponse;

            if (rating == null)
            {
                return new NotFoundResult();
            }

            var model = new RatingModel()
            {
                Id = rating.Id,
                ProductId = rating.ProductId,
                UserId = rating.UserId,
                Rating = rating.Rating,
                Timestamp = rating.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ssZ"),
                UserNotes = rating.UserNotes,
                LocationName = rating.LocationName,
            };

            return new OkObjectResult(model);
        }
    }
}
