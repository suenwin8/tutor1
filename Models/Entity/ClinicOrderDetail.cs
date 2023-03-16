using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    
    public class ClinicOrderDetail
    {  
        
        public int ClinicOrderDetailID { get; set; }
        public int Quantity { get; set; }
        public int ClinicOrderID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Product product { get; set; }
        public ClinicOrder Order { get; set; }
        
    }
}
