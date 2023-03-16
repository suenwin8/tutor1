using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tutor1.Extension;
using tutor1.Models;
using tutor1.Models.Entity;

namespace tutor1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected int currentCount = 0;
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected void IncrementCount()
        {
            currentCount++;
        }

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var pokemons = new List<Pokemon>()
            {
                new Pokemon()
                {
                    Id = 1,
                    Name = "水箭龜",
                    Property = "水系"
                },
                new Pokemon()
                {
                    Id = 2,
                    Name = "噴火龍",
                    Property = "火系"
                },
                new Pokemon()
                {
                    Id = 3,
                    Name = "妙蛙花",
                    Property = "草系"
                }
            };
            return View(pokemons);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ToDoItem()
        {
            var baseUrl = _httpContextAccessor.HttpContext?.Request.BaseUrl();
            ViewData["contentPath"] = baseUrl;
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
