using System;

namespace productsAPIs.Models
{
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
}
