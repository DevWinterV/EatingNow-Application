using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Ward")]
    public class EN_Ward
    {
        [Key]
        public int WardId { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual EN_District District { get; set; }
    }
}
