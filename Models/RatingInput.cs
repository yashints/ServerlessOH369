using Microsoft.WindowsAzure.Storage.Table;

namespace productsAPIs.Models
{
    public class RatingInput : TableEntity
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public int Rating { get; set; }
        public string UserNotes { get; set; }
    }
}
