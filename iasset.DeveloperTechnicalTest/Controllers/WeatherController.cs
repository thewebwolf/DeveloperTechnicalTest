using iasset.DeveloperTechnicalTest.Core;
using iasset.DeveloperTechnicalTest.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace iasset.DeveloperTechnicalTest.Controllers
{
    public class WeatherController : ApiController
    {
        private readonly IWeatherService weatherservice;

        public WeatherController(IWeatherService weatherservice)
        {

            if (weatherservice != null)
            {
                this.weatherservice = weatherservice;
            }
            else
            {
                throw new ArgumentNullException("WeatherService cannot be null");
            }
        }

        [Route("api/weather/{country}")]
        public IHttpActionResult GetCitiesByCountry(string country)
        {
            var cities = weatherservice.GetCitiesForCountry(country);

            if (cities == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cities);
            }
        }

        [Route("api/weather/{country}/{city}")]
        public IHttpActionResult GetWeatherForCountry(string country, string city)
        {
            var weather = weatherservice.GetWeatherForCity(new City() { CityName = city, Country = country });

            if (weather == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(weather);
            }
        }

    }
}
