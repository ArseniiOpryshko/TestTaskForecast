using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestTaskForecast.Models;

namespace TestTaskForecast.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string apiKey = "Vo7mq9XK3HlFSM0B6zqGfiAJxMVMPPlf";
        private const string citySearchUrl = "http://dataservice.accuweather.com/locations/v1/cities/search";
        private const string forecastUrl = "http://dataservice.accuweather.com/forecasts/v1/daily/1day/";

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetWeather(string cityName)
        {
            City city = await GetCityId(cityName);

            if (city == null)
            {
                ViewBag.ErrorMessage = "City not found.";
                return View("Index");
            }

            WeatherForecast weatherData = await GetWeatherForecast(city.Key);
            if (weatherData == null)
            {
                ViewBag.ErrorMessage = "Failed to get weather forecast.";
                return View("Index");
            }

            weatherData.Headline.Name = city.LocalizedName;

            if (weatherData.DailyForecasts.Any(forecast => forecast.Day.HasPrecipitation && forecast.Day.PrecipitationType == "Rain"))
            {
                ViewBag.AlertMessage = $"Attention! Rain is expected in the city {cityName}. " +
                    $"{weatherData.DailyForecasts.First().Day.IconPhrase}.";
            }

            return View("Index", weatherData);
        }

        private async Task<City> GetCityId(string cityName)
        {
            try
            {
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{citySearchUrl}?apikey={apiKey}&q={cityName}"));

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.FetchingError = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(content);
                    return cities?.FirstOrDefault();
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.FetchingError = $"Request error: {e.Message}";
            }

            return null;
        }

        private async Task<WeatherForecast> GetWeatherForecast(string cityId)
        {
            try
            {
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{forecastUrl}{cityId}?apikey={apiKey}"));

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.FetchingError = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherForecast>(content);
                }
            }
            catch (HttpRequestException e)
            {
                ViewBag.FetchingError = $"Request error: {e.Message}";
            }

            return null;
        }
    }
}