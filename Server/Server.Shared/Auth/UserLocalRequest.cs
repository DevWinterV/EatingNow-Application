using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Auth
{
    public class UserLocalRequest
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public string LocationCode { get; set; }
        public List<string> ListRole { get; set; }
    }
}
