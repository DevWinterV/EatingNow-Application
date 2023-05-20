using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.ChatRoom
{
    public class ChatDescriptionResponse
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Avarta { get; set; }
        public string ContentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
