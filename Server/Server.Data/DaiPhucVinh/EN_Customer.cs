using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Customer")]
    public class EN_Customer
    {
        [Key]
        public string CustomerId { get; set; }
        public string CompleteName { get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual EN_Province Province { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual EN_District District { get; set; }
        public int WardId { get; set; }
        [ForeignKey("WardId")]
        public virtual EN_Ward Ward { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
    }
}
