using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using productsAPIs.Models;
using System.Threading.Tasks;

namespace productsAPIs
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "/{ratingId}")] HttpRequest req,
            [Table("ratings", "RATING", "{ratingId}")] Rating rating,
            ILogger log)
        {
            log.LogInformation("GetRating HTTP trigger function processed a request.");

            if (rating == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(rating);
        }
    }
}
