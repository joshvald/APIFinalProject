using RestSharp;
using RestSharpProject.DataModels;
using RestSharpProject.Resource;
using RestSharpProject.Tests.TestData;

namespace RestSharpProject.Helpers
{
    /// <summary>
    /// Class containing all method for pets
    /// </summary>
    public class Helper
    {
        private static async Task<string> GetToken(RestClient restClient)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            var postRequest = new RestRequest(Endpoint.GenerateToken).AddJsonBody(AuthTokenDetails.credentials());

            var generateToken = await restClient.ExecutePostAsync<TokenModel>(postRequest);

            return generateToken.Data.Token;
        }

        /// <summary>
        /// Send Post request to add new booking
        /// </summary>
        /// <param name="restClient"></param>
        /// <returns></returns>
        public static async Task<RestResponse<BookingIdModel>> CreateBooking(RestClient restClient)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            var postRequest = new RestRequest(Endpoint.BaseBooking).AddJsonBody(GenerateBookingDetails.bookingDetails());

            return await restClient.ExecutePostAsync<BookingIdModel>(postRequest);
        }

        /// <summary>
        /// Send GET request
        /// </summary>
        /// <param name="restClient"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public static async Task<RestResponse<BookingDetailsModel>> GetBook(RestClient restClient, int bookingId)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            var getRequest = new RestRequest(Endpoint.BookingMethodById(bookingId));

            return await restClient.ExecuteGetAsync<BookingDetailsModel>(getRequest);
        }

        /// <summary>
        /// Deletes the booking
        /// </summary>
        public static async Task<RestResponse> DeleteBooking(RestClient restClient, int bookingId)
        {
            var token = await GetToken(restClient);
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultHeader("Cookie", "token=" + token);

            var getRequest = new RestRequest(Endpoint.BookingMethodById(bookingId));

            return await restClient.DeleteAsync(getRequest);
        }

        public static async Task<RestResponse<BookingDetailsModel>> UpdateBooking(RestClient restClient, BookingDetailsModel booking, int bookingId)
        {
            var token = await GetToken(restClient);
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultHeader("Cookie", "token=" + token);

            var putRequest = new RestRequest(Endpoint.BookingMethodById(bookingId)).AddJsonBody(booking);

            return await restClient.ExecutePutAsync<BookingDetailsModel>(putRequest);
        }

        
    }
}
