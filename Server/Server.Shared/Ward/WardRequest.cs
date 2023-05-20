using DaiPhucVinh.Shared.Common;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Ward
{
    public class WardRequest : BaseRequest
    {
        public int WardId { get; set; }
        public string Name { get; set; }
        public int ItemDistrictCode { get; set; }
        public string Term { get; set; }
    }
}
