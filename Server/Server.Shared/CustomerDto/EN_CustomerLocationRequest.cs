using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerDto
{
    public class EN_CustomerLocationRequest : BaseRequest
    {
        public string CustomerId { get; set; }
        public double Latitude { get; set; }
        public double Longittude { get; set; }
    }
}
