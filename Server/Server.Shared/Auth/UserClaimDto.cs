using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Auth
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserName { get; set; }   
        public string ClaimName { get; set; }      
        public string ClaimValue { get; set; }
        public bool Deleted { get; set; }
    }
}
