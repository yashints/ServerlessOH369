using FluentValidation;
using Microsoft.WindowsAzure.Storage.Table;
using productsAPIs.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace productsAPIs
{
    internal static class Helpers
    {
        public class RatingInputValidator : AbstractValidator<RatingInput>
        {
            public RatingInputValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.ProductId).NotEmpty();
                RuleFor(x => x.Rating).NotEmpty()
                  .GreaterThanOrEqualTo(0)
                  .LessThanOrEqualTo(5);
            }
        }
        public static async Task<Boolean> EntityExists(string url, string id)
        {
            HttpClient newClient = new HttpClient();
            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(url, id));
            HttpResponseMessage response = await newClient.SendAsync(newRequest);

            return response.StatusCode != System.Net.HttpStatusCode.BadRequest;
        }

        public static async Task<RatingResponse> WriteToTable(CloudTable outputTable, RatingInput input)
        {
            var response = new RatingResponse(input);
            var addEntryOperation = TableOperation.Insert(response);
            await outputTable.ExecuteAsync(addEntryOperation);
            return response;
        }
    }
}