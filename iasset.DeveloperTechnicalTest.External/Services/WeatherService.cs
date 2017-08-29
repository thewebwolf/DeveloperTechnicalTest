using iasset.DeveloperTechnicalTest.Core;
using iasset.DeveloperTechnicalTest.Core.Models;
using iasset.DeveloperTechnicalTest.External.webservicex.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace iasset.DeveloperTechnicalTest.External.Services
{
    public class WeatherService : IWeatherService
    {
        private webservicex.net.GlobalWeatherSoapClient client = new GlobalWeatherSoapClient("GlobalWeatherSoap");

        public IEnumerable<City> GetCitiesForCountry(string country)
        {
            List<City> cityList = new List<City>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(client.GetCitiesByCountry(country));

            XmlNodeList cityNodes = xmlDoc.GetElementsByTagName("City");

            for (int i = 0; i < cityNodes.Count; i++)
            {
                if (cityNodes[i].InnerText.Length > 0)
                {
                    cityList.Add(new City()
                    {
                        Country = cityNodes[i].ParentNode.ChildNodes[0].InnerText,
                        CityName = cityNodes[i].InnerText
                    });
                }
            }

            return cityList.AsEnumerable();
        }

        public Weather GetWeatherForCity(City city)
        {
            try
            {
                Weather weather = new Weather();

                string weatherData = client.GetWeather(city.CityName, city.Country);
                if (!weatherData.Equals("Data Not Found"))
                {
                }
                else
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        @"http://api.openweathermap.org/data/2.5/weather?q=" + city.CityName + "&units=metric&type=like&mode=xml&APPID=30fda63a76e55d858a2d442fe5476c6f");
                    request.Method = "get";
                    request.KeepAlive = true;
                    request.ContentType = "appication/xml";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string myResponse = "";
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        myResponse = sr.ReadToEnd();
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(myResponse);

                    weather.Time = Convert.ToDateTime(GetValueForTagAndAttribute(xmlDoc, "lastupdate", "value"));
                    weather.SkyCondition = GetValueForTagAndAttribute(xmlDoc, "clouds", "name");
                    weather.Pressure = string.Format("{0} {1}", GetValueForTagAndAttribute(xmlDoc, "pressure", "value"), GetValueForTagAndAttribute(xmlDoc, "pressure", "unit"));
                    weather.Temperature = string.Format("{0} C", GetValueForTagAndAttribute(xmlDoc, "temperature", "value"));
                    weather.RelativeHumidity = string.Format("{0} {1}", GetValueForTagAndAttribute(xmlDoc, "humidity", "value"), GetValueForTagAndAttribute(xmlDoc, "humidity", "unit"));
                    weather.Wind = string.Format("{0} {1}", GetValueForTagAndAttribute(xmlDoc, "direction", "value"), GetValueForTagAndAttribute(xmlDoc, "direction", "code"));
                    weather.Visibility = GetValueForTagAndAttribute(xmlDoc, "visibility", "value");
                    weather.DewPoint = "NA";
                }

                return weather;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetValueForTagAndAttribute(XmlDocument xmlDoc, string tag, string attribute)
        {
            try
            {
                var elemList = xmlDoc.GetElementsByTagName(tag);
                for (int i = 0; i < elemList.Count; i++)
                {
                    string attrVal = elemList[i].Attributes[attribute].Value;
                    return attrVal;
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }
    }
}