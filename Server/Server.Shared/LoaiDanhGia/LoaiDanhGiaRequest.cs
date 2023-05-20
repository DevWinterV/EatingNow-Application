using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Shared.LoaiDanhGia
{
    public class LoaiDanhGiaRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SoNgay_NhacNho { get; set; }
        public int SoLan_NhacNho { get; set; }
        public string Description { get; set; }
    }
}
