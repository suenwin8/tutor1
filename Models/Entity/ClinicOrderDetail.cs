using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Enum;

namespace tutor1.Models.Entity
{
    
    public class ClinicOrderDetail
    {  
        
        public int ClinicOrderDetailID { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        // Navigation properties
        public int ProductID { get; set; }
        public Product product { get; set; }
        public int ClinicOrderID { get; set; }
        public ClinicOrder Order { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime DateOfInjection { get; set; }
        //public MedicalType typeOfMedical { get; set; }

    }
}
