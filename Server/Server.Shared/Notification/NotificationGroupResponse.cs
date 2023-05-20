using DaiPhucVinh.Shared.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Notification
{
    public class NotificationGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string UserIds { get; set; }
        public List<object> ImageList { get; set; }
    }
}
