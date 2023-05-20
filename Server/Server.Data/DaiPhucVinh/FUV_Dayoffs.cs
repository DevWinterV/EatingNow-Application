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
    [Table("FUV_Dayoffs")]
    public class FUV_Dayoffs
    {
        [Key]
        public int Id { get; set; }
        public int DayRequests { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int DayApproved { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDescription { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
