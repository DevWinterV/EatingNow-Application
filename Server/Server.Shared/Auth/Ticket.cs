using System;
using System.Collections.Generic;
namespace DaiPhucVinh.Shared.Auth
{
    public class Ticket
    {
        public string Tenant { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public bool IsPersistent { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public string RoleSystem { get; set; }
        public bool IsActive { get; set; }
        public string LocationCode { get; set; }
    }
}
