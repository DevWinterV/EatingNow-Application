using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Users")]
    public class EN_Store
    {
        [Key]
        public int UserId { get; set; }
        public string AbsoluteImage { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string OpenTime { get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual EN_Province Province { get; set; }
        public int CuisineId { get; set; }
        [ForeignKey("CuisineId")]
        public virtual EN_Cuisine Cuisine { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Status { get; set; }
    }
}
