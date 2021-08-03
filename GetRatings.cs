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

namespace Company.Function
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string userIdAPIURL = Environment.GetEnvironmentVariable("UserAPIURL");
            string productAPIURL = Environment.GetEnvironmentVariable("ProductAPIURL");

            var validator = new Helpers.RatingInputValidator();
            log.LogInformation("C# HTTP trigger function processed a request.");
            var json = await req.ReadAsStringAsync();
            var rating = JsonConvert.DeserializeObject<Helpers.RatingInput>(json);
            var validationResult = validator.Validate(rating);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            // Validate user
            if (!(await Helpers.EntityExists(userIdAPIURL, rating.UserId)))
            {
                return new BadRequestObjectResult("User does not exist!");
            }

            if (!(await Helpers.EntityExists(productAPIURL, rating.ProductId)))
            {
                return new BadRequestObjectResult("Product does not exist!");
            }

            var response = await Helpers.WriteToTable(ratingTable, rating);

            return new OkObjectResult(response);
        }
    }
}
