using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.Entity;

namespace tutor1.Models.DTO
{
    public class ClinicOrderDTO
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
        public string OrderDetails { get; set; }




    }
}
