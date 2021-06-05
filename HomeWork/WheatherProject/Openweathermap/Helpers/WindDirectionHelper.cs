namespace Openweathermap.Helpers
{
    public static class WindDirectionHelper
    {
        public static string FormatWindDirection(string windDirection)
        {
            string resultDirection = "";
            switch (windDirection)
            {
                case "N":
                    resultDirection= "North";
                    break;
                case "E":
                    resultDirection = "East";
                    break;
                case "S":
                    resultDirection = "South";
                    break;
                case "W":
                    resultDirection = "West";
                    break;
                case "NE":
                    resultDirection = "Northeast";
                    break;
                case "SE":
                    resultDirection = "Southeast";
                    break;
                case "NW":
                    resultDirection = "NorthWest";
                    break;
                default:
                    resultDirection="Something strange";
                    break;
              }

            return resultDirection;
        }
    }
}
