using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.Entity
{
    
    public class AppSetting
    {
        //
        //public int AppSettingID { get; set; }
        //public int ClinicOrderId { get; set; }
        //public int seqid { get; set; }
        [Key]
        public int ID { get; set; }
        public string VARNAME { get; set; }
        public int INTVALUE { get; set; }
        public string TXTVALUE { get; set; }
        public string VARGROUP { get; set; }
    }
}
