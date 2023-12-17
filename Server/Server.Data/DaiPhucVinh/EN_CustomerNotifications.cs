using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("CustomerNotifications")]

    public class EN_CustomerNotifications
    {
        [Key]
        public int NotificationID { get; set; }
        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual EN_Customer Customer { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
        public string Action_Link { get; set; }

    }
}
