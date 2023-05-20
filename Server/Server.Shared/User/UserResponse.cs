using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public string RoleSystem { get; set; }
        public string RoleSystemName { get; set; }
        public bool Active { get; set; }

    }
}
