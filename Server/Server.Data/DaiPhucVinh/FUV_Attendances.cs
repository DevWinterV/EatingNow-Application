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
    [Table("FUV_Attendances")]
    public class FUV_Attendances
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string CheckInData { get; set; }
        public double? LongCheckIn { get; set; }
        public double? LatCheckIn { get; set; }
        public string LocationImagesCheckin { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string CheckOutData { get; set; }
        public double? LongCheckOut { get; set; }
        public double? LatCheckOut { get; set; }
        public string LocationImagesCheckout { get; set; }
        public string Note { get; set; }
        public bool? Accept { get; set; }
        public string NoteReply { get; set; }
        public string ReplyBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
