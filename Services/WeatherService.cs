using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Interfaces;
using tutor1.Models.Entity;

namespace tutor1.Services
{
    public class WeatherService : IWeather
    {
        public Weather GetWeather(string location, DateTime date)
        {
            var data = this.GetFakeWeatherData();
            return data.SingleOrDefault(x => x.Location == location
                && x.asDate == date);
        }

        public List<Weather> GetFakeWeatherData()
        {
            var fakeData = new List<Weather>()
        {
            new Weather()
            {
                asDate = new DateTime(2018, 10, 1),
                Location = "台北市",
                Humidity = "38%",
                Temperature = "24-28度C",
                RainProbability = "40%",
                Status = "晴天"
            },
            new Weather()
            {
                asDate = new DateTime(2018, 10, 1),
                Location = "桃園市",
                Temperature = "24-30度C",
                Humidity = "35%",
                RainProbability = "20%",
                Status = "晴天"
            },
            new Weather()
            {
                asDate = new DateTime(2018, 10, 1),
                Location = "宜蘭縣",
                Temperature = "24-28度C",
                Humidity = "80%",
                RainProbability = "65%",
                Status = "雨天"
            }
        };
            return fakeData;
        }
    }
}
