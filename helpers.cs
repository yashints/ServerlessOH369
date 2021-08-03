using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.WindowsAzure.Storage.Table;

namespace productsAPIs
{
  internal static class Helpers
  {
    public class RatingInput : TableEntity
    {
      public string UserId { get; set; }
      public string ProductId { get; set; }
      public string LocationName { get; set; }
      public int Rating { get; set; }
      public string UserNotes { get; set; }
    }
    public class RatingResponse : RatingInput
    {
      public string Id { get; set; }
      public RatingResponse(RatingInput input)
      {
        UserId = input.UserId;
        ProductId = input.ProductId;
        LocationName = input.LocationName;
        Rating = input.Rating;
        UserNotes = input.UserNotes;
        Id = Guid.NewGuid().ToString();
        PartitionKey = "RATING";
        RowKey = Id;
      }

      public RatingResponse()
      {

      }
    }
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