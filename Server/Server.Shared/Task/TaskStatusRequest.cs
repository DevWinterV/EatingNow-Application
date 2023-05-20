using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;
using System;

namespace DaiPhucVinh.Shared.Task
{
    public class TaskStatusRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
    }
}
