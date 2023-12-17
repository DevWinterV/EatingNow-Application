using DaiPhucVinh.Server.Data.DaiPhucVinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerNotification
{
    public class EN_CustomerNotificationResponse
    {
        public int NotificationID { get; set; }
        public string CustomerID { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
        public string Action_Link { get; set; }
    }
}
