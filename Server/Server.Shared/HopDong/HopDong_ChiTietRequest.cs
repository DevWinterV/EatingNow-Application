using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.HopDong
{
    public class HopDong_ChiTietRequest : BaseRequest
    {
        public int Id { get; set; }
        public int HopDong_Id { get; set; }
        public string ItemCode { get; set; }
        public string ChatLuong { get; set; }
        public string DonGia { get; set; }
        public double SoLuong { get; set; }
        public double ThanhTien { get; set; }
        public string GhiChu { get; set; }
        public int? Image_Id { get; set; }
        public int? ChatLuong_Id { get; set; }
        public DateTime NgayDaGiaoHang { get; set; }
    }
}
