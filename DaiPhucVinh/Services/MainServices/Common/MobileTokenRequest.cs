using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Common
{
    public class MobileTokenRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string PhoneOs { get; set; }
        public string DeviceToken { get; set; }
        public string LocationCode { get; set; }
    }
}
