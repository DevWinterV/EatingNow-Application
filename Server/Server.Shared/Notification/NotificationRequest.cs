using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Notification
{
    public class NotificationRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NotificationTypeId { get; set; }
        public int NotificationGroupId { get; set; }
        public bool IsSend { get; set; }
        public bool? IsSee { get; set; }
    }
}
