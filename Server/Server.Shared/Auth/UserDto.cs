using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos123.Shared.Auth
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public string LocationCode { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public string Roles { get; set; }
        public bool Deleted { get; set; }
        public bool IsMarket { get; set; }
    }
}
