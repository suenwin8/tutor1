using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    public class Weather
    {
        public string Location { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string RainProbability { get; set; }
        public string Status { get; set; }

        public DateTime asDate { get; set; }
    }
}
