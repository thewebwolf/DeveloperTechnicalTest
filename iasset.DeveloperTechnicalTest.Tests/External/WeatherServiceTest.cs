using iasset.DeveloperTechnicalTest.External.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace iasset.DeveloperTechnicalTest.Tests.External
{
    [TestClass]
    public class WeatherServiceTest
    {
        [TestMethod]
        public void TestCreateService()
        {
            try
            {
                WeatherService service = new WeatherService();
            }
            catch (Exception ex)
            {
                Assert.Fail("Could not create service");
            }
        }

        [TestMethod]
        public void GetCitiesTest()
        {
            WeatherService service = new WeatherService();

            var cities = service.GetCitiesForCountry("United Kingdom");

            Assert.AreNotEqual(cities.Count(), 0);

            var weather = service.GetWeatherForCity(cities.First(c => c.CityName.Contains("London")));

            Assert.IsNotNull(weather);
        }
    }
}