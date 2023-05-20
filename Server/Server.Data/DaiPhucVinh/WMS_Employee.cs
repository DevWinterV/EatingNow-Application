using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Employee")]
    public class WMS_Employee
    {
        public int Id { get; set; }
        [Key]
        public string EmployeeCode { get; set; }
        public string UserLogin { get; set; }
        public int? EmployeeGroup_Id { get; set; }
        public int FESUserId { get; set; }
        public string FullName { get; set; }
        public string DOB { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public int? ImageRecordId { get; set; }
        [ForeignKey("ImageRecordId")]
        public virtual Entity.ImageRecord ImageRecord { get; set; }
        public bool? IsAdministrator { get; set; }
        public string BranchCode { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public int? Department_Id { get; set; }
        public string Position { get; set; }
        public string Signature { get; set; }
    }
}
