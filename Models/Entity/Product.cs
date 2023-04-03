using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Enum;

namespace tutor1.Models.Entity
{
    public class Product
    {
             
        public int ProductID { get; set; }  
        
        public string Name { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime DateOfInjection { get; set; }
        //public MedicalType typeOfMedical { get; set; }

        public decimal Price { get; set; }
    }
}
