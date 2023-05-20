using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Province")]
    public class EN_Province
    {
        [Key]
        public int ProvinceId { get; set; }
        public string Name { get; set; }
    }
}
