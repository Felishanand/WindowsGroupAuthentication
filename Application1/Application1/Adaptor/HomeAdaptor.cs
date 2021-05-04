using Application1;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application1
{
    public class HomeAdaptor : IHomeAdaptor
    {   
        private readonly IOptions<EndPointsConfig> _config;

        public HomeAdaptor(IOptions<EndPointsConfig> config)
        {
            _config = config;
        }
        public List<WeatherForecast> GetGroup1Values()
        {
            return HttpClientHelper.Instance.GetAsync<List<WeatherForecast>>(_config.Value.POCMainEndpoint, Constants.poc_main_Group1Values);
        }
        public List<WeatherForecast> GetGroup2Values()
        {
            return HttpClientHelper.Instance.GetAsync<List<WeatherForecast>>(_config.Value.POCMainEndpoint, Constants.poc_main_Group2Values);
        }
        public List<WeatherForecast> GetBothValues()
        {
            return HttpClientHelper.Instance.GetAsync<List<WeatherForecast>>(_config.Value.POCMainEndpoint, Constants.poc_main_Both);
        }
    }
}
