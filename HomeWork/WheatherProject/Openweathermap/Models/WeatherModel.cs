using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Openweathermap.Models
{
    public class WeatherModel
    {
        public string Date { get; set; }
        public string City { get; set; }
        public double Temperature { get; set; }
        public string TemperatureMetric  { get; set; }
    }
}
