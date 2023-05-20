using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Chart
{
    public class ItemResponse
    {
        public int HopDong_Id { get; set; }
        public string ItemCode { get; set; }
        public double DonGia { get; set; }
        public double SoLuong { get; set; }
        public double ThanhTien { get; set; }
    }
}
