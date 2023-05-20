using System;

namespace DaiPhucVinh.Shared.Common
{
    public class BaseRequest
    {
        public string Term { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
    }
}
