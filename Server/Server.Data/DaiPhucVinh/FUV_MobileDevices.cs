using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_MobileDevices")]
    public class FUV_MobileDevices
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Server.Data.Entity.User User { get; set; }
        public string PhoneOs { get; set; }
        public string DeviceToken { get; set; }
        public DateTime? LastActive { get; set; }
        public bool? Active { get; set; }
    }
}
