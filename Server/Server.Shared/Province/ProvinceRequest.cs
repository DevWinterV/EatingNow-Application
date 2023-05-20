using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Shared.Province
{
    public class ProvinceRequest : BaseRequest
    {
        public int ProvinceId { get; set; }
        public string Name { get; set; }
    }
}
