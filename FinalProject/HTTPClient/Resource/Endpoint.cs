using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Resource
{
    /// <summary>
    /// Class containing all endpoints used in API tests
    /// </summary>
    public class Endpoint
    {
        // BASE URL
        public const string BaseURL = "https://restful-booker.herokuapp.com/";
        public const string BookingEndpoint = "booking";
        public const string AuthEndpoint = "auth";

        public static string GetURL(string endpoint) => $"{BaseURL}{endpoint}";
        public static Uri GetUri(string endpoint) => new Uri(GetURL(endpoint));
    }
}
