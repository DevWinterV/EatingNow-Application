using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Notification
{
    public class NotificationTypeRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
