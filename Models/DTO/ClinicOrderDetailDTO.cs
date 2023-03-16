using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Entity;

namespace tutor1.Models.DTO
{
    public class ClinicOrderDetailDTO
    {
        public int ClinicOrderDetailID { get; set; }
        public int Quantity { get; set; }
        public int ClinicOrderID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Product product { get; set; }
    }
}
