using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    public class WMS_AutoGenCodeConfig
    {
        [Key]
        public int Id { get; set; }
        public string TableName { get; set; }
        public int? Typekey { get; set; }
        public string Prefix { get; set; }
        public bool? IsYear { get; set; }
        public bool? IsMonth { get; set; }
        public bool? IsDay { get; set; }
        public int? AutonumberLenght { get; set; }
        public string Discription { get; set; }
        public int? CurrentCode { get; set; }
        public string NumberIsMissed { get; set; }
        public string Note { get; set; }
        public int? CurrentNumber { get; set; }
    }
}
