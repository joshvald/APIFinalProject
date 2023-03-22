using HTTPClient.DataModels;
using HTTPClient.Resource;
using HTTPClient.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace HTTPClient.Helpers
{
    /// <summary>
    /// Class containing all method for booking
    /// </summary>
    public class Helper
    {
        private HttpClient httpClient;

        public void HeaderAcceptJson()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task HeaderAuthenticate()
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");
        }

        /// <summary>
        /// Returns the auth token to use for access to the PUT and DELETE
        /// </summary>
        private async Task<string> GetToken()
        {
            HeaderAcceptJson();

            // Serialize Content
            var request = JsonConvert.SerializeObject(AuthTokenDetails.credentials());
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            var httpResponse = await httpClient.PostAsync(Endpoint.GetURL(Endpoint.AuthEndpoint), postRequest);

            // Deserialize Content
            var token = JsonConvert.DeserializeObject<TokenModel>(httpResponse.Content.ReadAsStringAsync().Result);

            // Return Token
            return token.Token;
        }

        /// <summary>
        /// Creates a new booking
        /// </summary>
        public async Task<HttpResponseMessage> CreateBooking()
        {
            HeaderAcceptJson();

            // Serialize Content
            var request = JsonConvert.SerializeObject(GenerateBookingDetails.bookingDetails());
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Return Send Request
            return await httpClient.PostAsync(Endpoint.GetURL(Endpoint.BookingEndpoint), postRequest);
        }

        /// <summary>
        /// Returns a specific booking based upon the booking id provided
        /// </summary>
        public async Task<HttpResponseMessage> GetBooking(int bookingId)
        {
            HeaderAcceptJson();

            // Return Get Request
            return await httpClient.GetAsync(Endpoint.GetUri($"{Endpoint.BookingEndpoint}/{bookingId}"));
        }

        /// <summary>
        /// Updates a current booking with a partial payload
        /// </summary>
        public async Task<HttpResponseMessage> UpdateBooking(BookingDetailsModel bookingDetails, int bookingId)
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");

            // Serialize Content
            var request = JsonConvert.SerializeObject(bookingDetails);
            var putRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Return Put Request
            return await httpClient.PutAsync(Endpoint.GetURL($"{Endpoint.BookingEndpoint}/{bookingId}"), putRequest);
        }

        /// <summary>
        /// Deletes the booking
        /// </summary>
        public async Task<HttpResponseMessage> DeleteBooking(int bookingId)
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");

            // Return Delete Request
            return await httpClient.DeleteAsync(Endpoint.GetURL($"{Endpoint.BookingEndpoint}/{bookingId}"));
        }
    }
}
