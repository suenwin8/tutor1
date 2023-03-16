using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    public class ClinicOrder
    {        
        public int ClinicOrderId { get; set; }
        public string clinicOrder_seqid { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfClinicOrder { get; set; }
        public bool seeDoctor { get; set; }
        public string customer { get; set; }       

        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdatedTime { get; set; }
        // Navigation property
        public ICollection<ClinicOrderDetail> OrderDetails { get; set; }
    }
}
