using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.HopDong
{
    public class HopDong_ChiTietResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Code { get; set; }
        public string ChatLuong { get; set; }

        public DateTime? NgayDaGiaoHang { get; set; }
        public double? price { get; set; }
        public double? qty { get; set; }
        public double? amt { get; set; }
        public string SoHopDong { get; set; }
        public DateTime?  NgayTaoHopDong { get; set; }
        public int? OrderBy { get; set; }

        public string ChatLuongName { get; set; }
    }
}
