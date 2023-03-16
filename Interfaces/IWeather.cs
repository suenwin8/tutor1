using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Entity;

namespace tutor1.Interfaces
{
    public interface IWeather
    {
        public Weather GetWeather(string location, DateTime date);
        public List<Weather> GetFakeWeatherData();
    }
}
