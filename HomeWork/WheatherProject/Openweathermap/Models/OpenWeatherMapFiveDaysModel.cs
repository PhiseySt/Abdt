using Newtonsoft.Json;
using System.Collections.Generic;

namespace Openweathermap.Models
{

    public class WeatherFiveDaysData
    {

        [JsonProperty("cod")]
        public string cod { get; set; }

        [JsonProperty("message")]
        public int message { get; set; }

        [JsonProperty("cnt")]
        public int cnt { get; set; }

        [JsonProperty("list")]
        public IList<List> list { get; set; }

        [JsonProperty("city")]
        public City city { get; set; }
    }

    public class List
{

    [JsonProperty("dt")]
    public int dt { get; set; }

    [JsonProperty("main")]
    public Main main { get; set; }

    [JsonProperty("weather")]
    public IList<Weather> weather { get; set; }

    [JsonProperty("clouds")]
    public Clouds clouds { get; set; }

    [JsonProperty("wind")]
    public Wind wind { get; set; }

    [JsonProperty("visibility")]
    public int visibility { get; set; }

    [JsonProperty("pop")]
    public double pop { get; set; }

    [JsonProperty("sys")]
    public Sys sys { get; set; }

    [JsonProperty("dt_txt")]
    public string dt_txt { get; set; }

}

public class City
{

    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("name")]
    public string name { get; set; }

    [JsonProperty("coord")]
    public Coord coord { get; set; }

    [JsonProperty("country")]
    public string country { get; set; }

    [JsonProperty("population")]
    public int population { get; set; }

    [JsonProperty("timezone")]
    public int timezone { get; set; }

    [JsonProperty("sunrise")]
    public int sunrise { get; set; }

    [JsonProperty("sunset")]
    public int sunset { get; set; }
}


}
