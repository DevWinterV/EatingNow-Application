using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_HopDong_Templete")]
    public class WMS_HopDong_Templete
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }   
}
