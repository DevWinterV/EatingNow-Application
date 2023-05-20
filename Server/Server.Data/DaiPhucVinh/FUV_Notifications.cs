using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_Notifications")]
    public class FUV_Notifications
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NotificationTypeId { get; set; }
        [ForeignKey("NotificationTypeId")]
        public virtual FUV_NotificationTypes FUV_NotificationTypes { get; set; }
        public int NotificationGroupId { get; set; }
        [ForeignKey("NotificationGroupId")]
        public virtual FUV_NotificationGroups FUV_NotificationGroups { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsSend { get; set; }
        public DateTime? SendAt { get; set; }
        public bool? IsSee { get; set; }
    }
}
