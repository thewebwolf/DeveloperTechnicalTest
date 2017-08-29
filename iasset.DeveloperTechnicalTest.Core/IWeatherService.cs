using iasset.DeveloperTechnicalTest.Core.Models;
using System.Collections.Generic;

namespace iasset.DeveloperTechnicalTest.Core
{
    public interface IWeatherService
    {
        IEnumerable<City> GetCitiesForCountry(string country);

        Weather GetWeatherForCity(City city);
    }
}