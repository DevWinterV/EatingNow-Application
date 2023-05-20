
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_HopDong")]
    public class WMS_HopDong
    {
        [Key]
        public int Id { get; set; }
        public string KhachHang_Code { get; set; }
        [ForeignKey("KhachHang_Code")]
        public virtual WMS_Customers WMS_Customer { get; set; }
        public int? BaoGia_Id { get; set; }
        [ForeignKey("BaoGia_Id")]
        public virtual WMS_Quotation WMS_Quotation { get; set; }
        public string SoHopDong { get; set; }
        public DateTime? NgayKy { get; set; }
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
        public double TongTien_TruocThue { get; set; }
        public double PhanTramThue { get; set; }
        public double TienThue { get; set; }
        public double? TongTien { get; set; }
        public string Dieu1 { get; set; }
        public string Dieu2 { get; set; }
        public string Dieu3 { get; set; }
        public string Dieu4 { get; set; }
        public string Dieu5 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? ChuyenTien1_Ngay { get; set; }
        public double? ChuyenTien1_SoTien { get; set; }
        public DateTime? ChuyenTien2_Ngay { get; set; }
        public double? ChuyenTien2_SoTien { get; set; }
        public DateTime? ChuyenTien3_Ngay { get; set; }
        public double? ChuyenTien3_SoTien { get; set; }
        public DateTime? NgayBatDauGiaoHang { get; set; }
        public bool? IsDaKy { get; set; }
        public DateTime? NgayTaoHopDong { get; set; }
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public virtual WMS_Employee WMS_Employee { get; set; }
        public double? SoTienConLaiSauThanhToan { get; set; }
        public bool? IsTheoDoiCongNo { get; set; }
        public string Dieu6 { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }

        public virtual ICollection<WMS_HopDong_ChiTiet> WMS_HopDong_ChiTiet { get; set; }
    }
}
