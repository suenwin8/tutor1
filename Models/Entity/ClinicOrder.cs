using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), Required, StringLength(30)]
        public string customer { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]        
        public DateTime LastUpdatedTime { get; set; }
        // Navigation property
        public List<ClinicOrderDetail> OrderDetails { get; set; }
    }
}
