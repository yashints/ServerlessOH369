
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace productsAPIs {
  internal static class Helpers {
    public static async Task<Boolean> EntityExists(string url, string id)
    {
      HttpClient newClient = new HttpClient();
      HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, string.Format(url, id));
      HttpResponseMessage response = await newClient.SendAsync(newRequest);

      return response.StatusCode != System.Net.HttpStatusCode.BadRequest;
    } 
  }
}