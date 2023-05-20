using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;
using System;

namespace DaiPhucVinh.Shared.Video
{
    public class VideoRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
