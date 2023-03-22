using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpProject.Resource
{
    class Endpoint
    {
        // BASE URL
        public const string BaseURL = "https://restful-booker.herokuapp.com/";
        public const string BookingEndpoint = "booking";
        public const string AuthEndpoint = "auth";

        public static string BaseBooking => $"{BaseURL}{BookingEndpoint}";
        public static string GenerateToken => $"{BaseURL}{AuthEndpoint}";

        public static string BookingMethodById(int bookingId) => $"{BaseBooking}/{bookingId}";
    }
}
