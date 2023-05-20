using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;
using System;

namespace DaiPhucVinh.Shared.Post
{
    public class PostRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public int Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
