using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    
    public class AppSetting
    {
        [Key]
        public int AppSettingID { get; set; }
        public int ClinicOrderId { get; set; }
        public int seqid { get; set; }
    }
}
