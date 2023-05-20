using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.HopDong
{
    public class HopDongResponse
    {
        //list
        public int Id { get; set; }
        public string SoHopDong { get; set; }
        public int? BaoGia_Id { get; set; }
        public string quotationName { get; set; }
        public string DocumentNo { get; set; }
        public string KhachHang_Code { get; set; }
        public string KhachHang_Name { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public double TongTien { get; set; }
        //detail
        public string BenA_TenCongTy { get; set; }
        public string BenA_DiaChi { get; set; }
        public string BenA_SoDienThoai { get; set; }
        public string BenA_Fax { get; set; }
        public string BenA_Email { get; set; }
        public string BenA_MST { get; set; }
        public string BenA_SoTaiKhoanNganHang { get; set; }
        public string BenA_NguoiDaiDien { get; set; }
        public string BenA_ChucVu { get; set; }
        public string BenB_TenCongTy { get; set; }
        public string BenB_DiaChi { get; set; }
        public string BenB_SoDienThoai { get; set; }
        public string BenB_Fax { get; set; }
        public string BenB_Email { get; set; }
        public string BenB_MST { get; set; }
        public string BenB_SoTaiKhoanNganHang { get; set; }
        public string BenB_NguoiDaiDien { get; set; }
        public string BenB_ChucVu { get; set; }
        public DateTime NgayKy { get; set; }
        public DateTime NgayTaoHopDong { get; set; }
        public double TienThue { get; set; }
        public double PhanTramThue { get; set; }
        public double TongTien_TruocThue { get; set; }
        public DateTime ChuyenTien1_Ngay { get; set; }
        public double ChuyenTien1_SoTien { get; set; }
        public double ChuyenTien2_SoTien { get; set; }
        public double ChuyenTien3_SoTien { get; set; }
        public DateTime NgayBatDauGiaoHang { get; set; }
        public string LocationCode { get; set; }
        public string Code { get; set; }
        public string LocationName { get; set; }
        public string Dieu2 { get; set; }
        public string Dieu3 { get; set; }
        public string Dieu4 { get; set; }
        public string Dieu5 { get; set; }
        public string Dieu6 { get; set; }

        // itemcheck
        public string Name { get; set; }
        public double price { get; set; }
        public double qty { get; set; }
        public double amt { get; set; }
        public List<HopDong_ChiTietResponse> ListHD_CT { get; set; }

    }

}
