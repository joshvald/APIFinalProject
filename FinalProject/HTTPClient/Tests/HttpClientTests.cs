using HTTPClient.DataModels;
using HTTPClient.Helpers;
using HTTPClient.Resource;
using HTTPClient.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace HTTPClient.Tests
{
    [TestClass]
    public class HttpClientTests
    {
        public Helper bookingHelper;
        
        public HttpClient httpClient;

        private readonly List<BookingIdModel> bookingCleanUpList = new List<BookingIdModel>();

        [TestInitialize]
        public async Task TestInitialize()
        {
            bookingHelper = new Helper();
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            foreach (var data in bookingCleanUpList)
            {
                var httpResponse = await bookingHelper.DeleteBooking(data.BookingId);
            }
        }

        /// <summary>
        /// Create a test method that creates a booking
        /// </summary>
        [TestMethod]
        public async Task CreateBooking()
        {
            // Create Data
            var addBooking = await bookingHelper.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingIdModel>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            bookingCleanUpList.Add(getResponse);

            // Get Data - Deserialize Content
            var getBooking = await bookingHelper.GetBooking(getResponse.BookingId);
            var getBookingResponse = JsonConvert.DeserializeObject<BookingDetailsModel>(getBooking.Content.ReadAsStringAsync().Result);

            var expectedBookingDetails = GenerateBookingDetails.bookingDetails();

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, addBooking.StatusCode, "Status code is not equal to 200");

            Assert.AreEqual(expectedBookingDetails.Firstname, getBookingResponse.Firstname, "First name does not match");
            Assert.AreEqual(expectedBookingDetails.Lastname, getBookingResponse.Lastname, "Last name does not match");
            Assert.AreEqual(expectedBookingDetails.Totalprice, getBookingResponse.Totalprice, "Total price does not match");
            Assert.AreEqual(expectedBookingDetails.Depositpaid, getBookingResponse.Depositpaid, "Deposit paid does not match");
            Assert.AreEqual(expectedBookingDetails.Bookingdates.Checkin.Date, getBookingResponse.Bookingdates.Checkin.Date, "Check in does not match");
            Assert.AreEqual(expectedBookingDetails.Bookingdates.Checkout.Date, getBookingResponse.Bookingdates.Checkout.Date, "Check out does not match");
            Assert.AreEqual(expectedBookingDetails.Additionalneeds, getBookingResponse.Additionalneeds, "Additional needs does not match");
        }

        /// <summary>
        /// Create a test method that updates the first and last name of the booking
        /// Note: Fix the assertion of booking dates
        /// </summary>
        [TestMethod]
        public async Task UpdateBooking()
        {
            // Create Data
            var addBooking = await bookingHelper.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingIdModel>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            bookingCleanUpList.Add(getResponse);

            // Get Data - Deserialize Content
            var getBooking = await bookingHelper.GetBooking(getResponse.BookingId);
            var getBookingResponse = JsonConvert.DeserializeObject<BookingDetailsModel>(getBooking.Content.ReadAsStringAsync().Result);

            // Update Data
            var updateBookingDetails = new BookingDetailsModel()
            {
                Firstname = "Updated First Name",
                Lastname = "Updated Last Name",
                Totalprice = getBookingResponse.Totalprice,
                Depositpaid = getBookingResponse.Depositpaid,
                Bookingdates = getBookingResponse.Bookingdates,
                Additionalneeds = getBookingResponse.Additionalneeds
            };

            var updateBooking = await bookingHelper.UpdateBooking(updateBookingDetails, getResponse.BookingId);
            var getUpdatedResponse = JsonConvert.DeserializeObject<BookingDetailsModel>(updateBooking.Content.ReadAsStringAsync().Result);

            // Get Updated Data - Deserialize Content
            var getUpdatedBooking = await bookingHelper.GetBooking(getResponse.BookingId);
            var getUpdatedBookingResponse = JsonConvert.DeserializeObject<BookingDetailsModel>(getUpdatedBooking.Content.ReadAsStringAsync().Result);

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, updateBooking.StatusCode);

            Assert.AreEqual(updateBookingDetails.Firstname, getUpdatedBookingResponse.Firstname, "First name does not match");
            Assert.AreEqual(updateBookingDetails.Lastname, getUpdatedBookingResponse.Lastname, "Last name does not match");
            Assert.AreEqual(updateBookingDetails.Totalprice, getUpdatedBookingResponse.Totalprice, "Total price does not match");
            Assert.AreEqual(updateBookingDetails.Depositpaid, getUpdatedBookingResponse.Depositpaid, "Deposit paid does not match");
            Assert.AreEqual(updateBookingDetails.Bookingdates.Checkin.Date, getUpdatedBookingResponse.Bookingdates.Checkin.Date, "Check in does not match");
            Assert.AreEqual(updateBookingDetails.Bookingdates.Checkout.Date, getUpdatedBookingResponse.Bookingdates.Checkout.Date, "Check out does not match");
            Assert.AreEqual(updateBookingDetails.Additionalneeds, getUpdatedBookingResponse.Additionalneeds, "Additional needs does not match");
        }

        /// <summary>
        /// Create a test method that delete a booking
        /// </summary>
        [TestMethod]
        public async Task DeleteBooking()
        {
            // Create Data
            var addBooking = await bookingHelper.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingIdModel>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            bookingCleanUpList.Add(getResponse);

            // Get Data - Deserialize Content
            var getBooking = await bookingHelper.GetBooking(getResponse.BookingId);
            var getBookingResponse = JsonConvert.DeserializeObject<BookingDetailsModel>(getBooking.Content.ReadAsStringAsync().Result);

            // Delete Data
            var deleteBooking = await bookingHelper.DeleteBooking(getResponse.BookingId);

            // Assertion
            Assert.AreEqual(HttpStatusCode.Created, deleteBooking.StatusCode);
        }

        /// <summary>
        /// Create a test method that checks if booking is invalid
        /// </summary>
        [TestMethod]
        public async Task InvalidBooking()
        {
            // Get Data - Deserialize Content
            var getBooking = await bookingHelper.GetBooking(1928463516);

            // Assertion
            Assert.AreEqual(HttpStatusCode.NotFound, getBooking.StatusCode);
        }
    }
}
