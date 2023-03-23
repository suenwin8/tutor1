using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdatedTime { get; set; }
        // Navigation property
        
        public string json_OrderDetails { get; set; }

        private List<ClinicOrderDetail> _OrderDetails; // field
        public List<ClinicOrderDetail> OrderDetails   // property
        {
            get { return _OrderDetails; }
            set { _OrderDetails = value; }
        }





    }
}
