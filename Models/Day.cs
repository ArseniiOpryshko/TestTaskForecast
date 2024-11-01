using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTaskForecast.Models
{
    public class Day
    {
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
    }
}