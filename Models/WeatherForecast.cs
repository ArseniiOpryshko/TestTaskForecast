using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTaskForecast.Models
{
    public class WeatherForecast
    {
        public Headline Headline { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; }
    }
}