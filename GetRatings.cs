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
using System.Collections.Generic;

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
            string userIdAPIURL = Environment.GetEnvironmentVariable("UserAPIURL");



            log.LogInformation("GetRating HTTP trigger function processed a request.");

            string userId = req.Query["userId"];

            // Validate user
            if (!(await Helpers.EntityExists(userIdAPIURL, userId)))
            {
                return new NotFoundResult();
            }


            var ratings = new List<RatingResponse>();
            string filter = TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId);
            var query = new TableQuery<RatingResponse>().Where(filter);
            TableContinuationToken continuationToken = null;

            do
            {
                var page = await ratingTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = page.ContinuationToken;
                ratings.AddRange(page.Results);
            }
            while (continuationToken != null);

            var result = ratings.Select(rating => new RatingModel()
            {
                Id = rating.Id,
                ProductId = rating.ProductId,
                UserId = rating.UserId,
                Rating = rating.Rating,
                Timestamp = rating.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ssZ"),
                UserNotes = rating.UserNotes,
                LocationName = rating.LocationName,
            });

            return new OkObjectResult(result);
        }
    }
}
