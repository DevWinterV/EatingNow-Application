using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.ChatRoom
{
    public class ChatDescriptionRequest : BaseRequest
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string ContentText { get; set; }
    }
}
