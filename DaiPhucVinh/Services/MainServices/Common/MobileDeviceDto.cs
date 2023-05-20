using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Common
{
    public class MobileDeviceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PhoneOs { get; set; }
        public string DeviceToken { get; set; }
        public DateTime? LastActive { get; set; }
    }
}
