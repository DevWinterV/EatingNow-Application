using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;

namespace DaiPhucVinh.Shared.User
{
    public class SimpleUserRequest : BaseRequest
    {
        public int? Id { get; set; }
    }
}
