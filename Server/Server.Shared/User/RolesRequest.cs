using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.User
{
    public class RolesRequest : BaseRequest
    {
        public bool Active { get; set; }
    }
}
