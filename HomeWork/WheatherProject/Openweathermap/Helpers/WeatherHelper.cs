﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Openweathermap.Extensions
{
    public static class WeatherHelper
    {
        public static string DegToCompass(int degree)
        {
            string[] arr = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            degree = (int)(degree / 22.5 + 0.5);
            return arr[degree % 16];
        }
    }
}
