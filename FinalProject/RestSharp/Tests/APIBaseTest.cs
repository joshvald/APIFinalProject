using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpProject.DataModels;

namespace RestSharpProject.Tests
{
    public class APIBaseTest
    {
        public RestClient RestClient { get; set; }
        public BookingIdModel BookingDetails { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            RestClient = new RestClient();
        }
    }
}
