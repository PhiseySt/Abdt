namespace Openweathermap.Helpers
{
    public class MetricHelper
    {
        public static double CToF(double celValue) => celValue * 9 / 5 + 32;

        public static double FtoC(double farValue) => (farValue - 32) * 5 / 9;
    }
}
