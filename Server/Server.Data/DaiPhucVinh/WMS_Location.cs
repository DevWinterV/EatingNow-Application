using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Location")]
    public class WMS_Location
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? City { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string EmloyeeCode { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public byte[] Image { get; set; }
        public double? Left_Image { get; set; }
        public double? Top_Image { get; set; }
        public int? Column_Image { get; set; }
        public int? Row_Image { get; set; }
        public string LocationFullName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerRole { get; set; }
        public string TaxCode { get; set; }
        public string BankingCode { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public string Note { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
