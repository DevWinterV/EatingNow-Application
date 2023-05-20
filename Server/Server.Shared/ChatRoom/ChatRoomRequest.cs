using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.ChatRoom
{
    public class ChatRoomRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Members { get; set; }
        public List<EmployeeResponse> listUser { get; set; }
    }
}
