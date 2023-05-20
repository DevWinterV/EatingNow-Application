using DaiPhucVinh.Server.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_Checkin")]
    public class FUV_Checkin
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public DateTime TimeCheckin { get; set; }
        public bool Deleted { get; set; }
    }
}
