using DaiPhucVinh.Shared.Employee;
using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Notification
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public int NotificationGroupId { get; set; }
        public string NotificationGroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsSend { get; set; }
        public bool IsSee { get; set; }
        public DateTime? SendAt { get; set; }
    }
}
