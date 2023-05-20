using DaiPhucVinh.Shared.Common;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.District
{
    public class DistrictRequest : BaseRequest
    {
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public int ItemProvinceCode { get; set; }
        public string Term { get; set; }
    }
}
