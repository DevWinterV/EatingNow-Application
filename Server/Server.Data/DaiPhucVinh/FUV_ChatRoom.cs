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
    [Table("FUV_ChatRoom")]
    public class FUV_ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public int UserOwnerId { get; set; }
        [ForeignKey("UserOwnerId")]
        public virtual User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
