using HTTPClient.DataModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Tests.TestData
{
    public class GenerateBookingDetails
    {
        public static BookingDetailsModel bookingDetails()
        {
            DateTime dt = DateTime.UtcNow.AddHours(+8).ToUniversalTime();

            Bookingdates bookingDates = new Bookingdates();
            bookingDates.Checkin = dt;
            bookingDates.Checkout = dt.AddDays(1);

            return new BookingDetailsModel
            {
                Firstname = "Joshua",
                Lastname = "Valdez",
                Totalprice = 111,
                Depositpaid = true,
                Bookingdates = bookingDates,
                Additionalneeds = "Breakfast"
            };
        }
    }
}
