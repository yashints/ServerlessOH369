using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using productsAPIs.Models;
using System.Threading.Tasks;

namespace productsAPIs
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("ratings")] CloudTable ratingTable,
            ILogger log)
        {
            log.LogInformation("GetRating HTTP trigger function processed a request.");

            string ratingId = req.Query["ratingId"];

            TableOperation tableOperation =
                TableOperation.Retrieve<RatingResponse>("RATING", ratingId);
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
