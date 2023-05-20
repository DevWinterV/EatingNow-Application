using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.WorkLog
{
    public class WorkLogResponse
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
