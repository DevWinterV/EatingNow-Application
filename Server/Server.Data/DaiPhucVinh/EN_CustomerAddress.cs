using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("CustomerAddress")]
    public class EN_CustomerAddress
    {
        [Key]
        public int AddressId { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual EN_Customer EN_Customer { get; set; }
        public string Name_Address { get; set; }
        public string Format_Address { get; set; }
        public string CustomerName { get; set; }
        public string PhoneCustomer { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Defaut { get; set; }
        public int ProvinceId { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual EN_Province Province { get; set; }
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual EN_District District { get; set; }
        public int WardId { get; set; }

        [ForeignKey("WardId")]
        public virtual EN_Ward Ward { get; set; }
    }
}
