using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTaskForecast.Models
{
    public class DailyForecast
    {
        public string Date { get; set; }
        public Day Day { get; set; }
        public Temperature Temperature { get; set; }
    }
}