using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpProject.DataModels;
using RestSharpProject.Resource;
using RestSharpProject.Helpers;
using System.Net;
using RestSharpProject.Tests.TestData;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace RestSharpProject.Tests
{
    [TestClass]
    public class RestSharpTests : APIBaseTest
    {
        private readonly List<BookingIdModel> bookingCleanupList = new List<BookingIdModel>();

        [TestInitialize]
        public async Task TestInitialize()
        {
            // Create Data
            var restResponse = await Helper.CreateBooking(RestClient);
            BookingDetails = restResponse.Data;

            // Assertion
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            foreach (var data in bookingCleanupList)
            {
                //var deleteBookingRequest = new RestRequest(Endpoint.BookingMethodById(data.BookingId));
                var deleteBookingResponse = await Helper.DeleteBooking(RestClient, data.BookingId);
            }
        }

        /// <summary>
        /// Create a test method that creates a booking
        /// </summary>
        [TestMethod]
        public async Task CreateBooking()
        {
            // Create Data
            var getBookingResponse = await Helper.GetBook(RestClient, BookingDetails.BookingId);

            // Clean Up
            bookingCleanupList.Add(BookingDetails);

            // Assertion
            var expectedBookingDetails = GenerateBookingDetails.bookingDetails();

            Assert.AreEqual(expectedBookingDetails.Firstname, getBookingResponse.Data.Firstname, "First name does not match");
            Assert.AreEqual(expectedBookingDetails.Lastname, getBookingResponse.Data.Lastname, "Last name does not match");
            Assert.AreEqual(expectedBookingDetails.Totalprice, getBookingResponse.Data.Totalprice, "Total price does not match");
            Assert.AreEqual(expectedBookingDetails.Depositpaid, getBookingResponse.Data.Depositpaid, "Deposit paid does not match");
            Assert.AreEqual(expectedBookingDetails.Bookingdates.Checkin.Date, getBookingResponse.Data.Bookingdates.Checkin.Date, "Check in does not match");
            Assert.AreEqual(expectedBookingDetails.Bookingdates.Checkout.Date, getBookingResponse.Data.Bookingdates.Checkout.Date, "Check out does not match");
            Assert.AreEqual(expectedBookingDetails.Additionalneeds, getBookingResponse.Data.Additionalneeds, "Additional needs does not match");
        }

        /// <summary>
        /// Create a test method that updates the first and last name of the booking
        /// Note: Fix the assertion of booking dates
        /// </summary>
        [TestMethod]
        public async Task UpdateBooking()
        {
            // Create Data
            var getBookingResponse = await Helper.GetBook(RestClient, BookingDetails.BookingId);

            // Clean Up
            bookingCleanupList.Add(BookingDetails);

            // Update Data 
            var updateBookingDetails = new BookingDetailsModel()
            {
                Firstname = "SampleFNameUpdated",
                Lastname = "SampleLNameUpdated",
                Totalprice = getBookingResponse.Data.Totalprice,
                Depositpaid = getBookingResponse.Data.Depositpaid,
                Bookingdates = getBookingResponse.Data.Bookingdates,
                Additionalneeds = getBookingResponse.Data.Additionalneeds
            };
            var updateBooking = await Helper.UpdateBooking(RestClient, updateBookingDetails, BookingDetails.BookingId);

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, updateBooking.StatusCode);

            // Get Updated Data 
            var getUpdatedBookingResponse = await Helper.GetBook(RestClient, BookingDetails.BookingId);

            // Assertion
            Assert.AreEqual(updateBookingDetails.Firstname, getUpdatedBookingResponse.Data.Firstname, "First name does not match");
            Assert.AreEqual(updateBookingDetails.Lastname, getUpdatedBookingResponse.Data.Lastname, "Last name does not match");
            Assert.AreEqual(updateBookingDetails.Totalprice, getUpdatedBookingResponse.Data.Totalprice, "Total price does not match");
            Assert.AreEqual(updateBookingDetails.Depositpaid, getUpdatedBookingResponse.Data.Depositpaid, "Deposit paid does not match");
            Assert.AreEqual(updateBookingDetails.Bookingdates.Checkin.Date, getUpdatedBookingResponse.Data.Bookingdates.Checkin.Date, "Check in does not match");
            Assert.AreEqual(updateBookingDetails.Bookingdates.Checkout.Date, getUpdatedBookingResponse.Data.Bookingdates.Checkout.Date, "Check out does not match");
            Assert.AreEqual(updateBookingDetails.Additionalneeds, getUpdatedBookingResponse.Data.Additionalneeds, "Additional needs does not match");
        }

        /// <summary>
        /// Create a test method that delete a booking
        /// </summary>
        [TestMethod]
        public async Task DeleteBooking()
        {
            // Delete Data
            var deleteBooking = await Helper.DeleteBooking(RestClient, BookingDetails.BookingId);

            // Assertion
            Assert.AreEqual(HttpStatusCode.Created, deleteBooking.StatusCode);
        }

        /// <summary>
        /// Create a test method that checks if booking is invalid
        /// </summary>
        [TestMethod]
        public async Task InvalidBooking()
        {
            // Create Data
            var getCreatedBooking = await Helper.GetBook(RestClient, 123456789);

            // Clean up
            bookingCleanupList.Add(BookingDetails);

            // Assertion
            Assert.AreEqual(HttpStatusCode.NotFound, getCreatedBooking.StatusCode);

            
        }
    }
}
