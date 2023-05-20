using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;

namespace DaiPhucVinh.Shared.User
{
    public class UserRequest : BaseRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string MaSoThue { get; set; }
        public string NguoiDaiDien { get; set; }
        public string AvatarUrl { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
        public string Roles { get; set; }
        public string RoleSystem { get; set; }
        public bool Deleted { get; set; }

        public string CustomerCode { get; set; }
    }
}
