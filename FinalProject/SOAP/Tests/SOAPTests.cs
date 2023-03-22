using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceReference1;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace SOAP.Tests
{
    [TestClass]
    public class SOAPTests
    {
        private readonly CountryInfoServiceSoapTypeClient countryTest =
            new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

        private tCountryCodeAndName[] CountryList()
        {
            var countryList = countryTest.ListOfCountryNamesByCode();
            return countryList;
        }

        private static tCountryCodeAndName RandomCountryCode(tCountryCodeAndName[] countryList)
        {
            Random rd = new Random();
            int countryCount = countryList.Count() - 1;
            int randomNum = rd.Next(0, countryCount);
            var randomCountryCode = countryList[randomNum];

            return randomCountryCode;
        }

        [TestMethod]
        public void CountryDetails()
        {
            var countryList = CountryList();
            var countryListCode = RandomCountryCode(countryList);
            var countryDetails = countryTest.FullCountryInfo(countryListCode.sISOCode);


            Assert.AreEqual(countryListCode.sISOCode, countryDetails.sISOCode, "Country code doesn't match.");
            Assert.AreEqual(countryListCode.sName, countryDetails.sName, "Country name doesn't match.");
        }

        [TestMethod]
        public void FiveCountryRecords()
        {
            var countryList = CountryList();
            List<tCountryCodeAndName> fiveRandomCountry = new List<tCountryCodeAndName>();

            for (int x = 0; x < 5; x++)
            {
                fiveRandomCountry.Add(RandomCountryCode(countryList));
            }

            foreach (var country in fiveRandomCountry)
            {
                var countryISOCode = countryTest.CountryISOCode(country.sName);
                Assert.AreEqual(country.sISOCode, countryISOCode, "Country code doesn't match.");
            }

        }
    }
}
