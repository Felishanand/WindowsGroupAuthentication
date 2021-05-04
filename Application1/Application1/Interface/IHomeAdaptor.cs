using System.Collections.Generic;

namespace Application1
{
    public interface IHomeAdaptor
    {
        List<WeatherForecast> GetGroup1Values();
        List<WeatherForecast> GetGroup2Values();
        List<WeatherForecast> GetBothValues();
    }
}