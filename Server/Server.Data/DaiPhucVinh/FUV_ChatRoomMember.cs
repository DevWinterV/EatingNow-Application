using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_ChatRoomMember")]
    public class FUV_ChatRoomMember
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual FUV_ChatRoom FUV_ChatRoom { get; set; }
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public virtual WMS_Employee WMS_Employee { get; set; }
    }
}
