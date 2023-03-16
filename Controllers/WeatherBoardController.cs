using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tutor1.Interfaces;
using tutor1.Services;

namespace tutor1.Controllers
{
    public class WeatherBoardController : Controller
    {
        private IWeather weather;
        public WeatherBoardController(IWeather _weather)
        {
            weather = _weather;
        }

        public IActionResult Invoke(string location, DateTime date)
        {
            var view_weather = weather.GetWeather(location, date);
            return View(view_weather);
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET: /HelloWorld/
        public string Index()
        {
            return "This is weather index...";
        }
        // 
        // GET: /WeatherBoard/Welcome/ 

        //public string Welcome()
        //{
        //    return "This is the Welcome action method...";
        //}

        //public string Welcome(string name, int numTimes = 1)
        //{
        //    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        //}

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}
