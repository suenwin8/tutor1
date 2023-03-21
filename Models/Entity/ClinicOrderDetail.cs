using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    
    public class ClinicOrderDetail
    {  
        
        public int ClinicOrderDetailID { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public int Quantity { get; set; }
        public int ClinicOrderID { get; set; }
        public int ProductID { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        // Navigation properties
        public Product product { get; set; }
        public ClinicOrder Order { get; set; }
        
    }
}
