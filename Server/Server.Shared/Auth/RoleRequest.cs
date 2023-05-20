namespace DaiPhucVinh.Shared.Auth
{
    public class RoleRequest
    {
        public string SystemName { get; set; }
        public string Display { get; set; }
        public bool ActiveRole { get; set; }

        //Danh mục
        public string ValueRole_NhomSanPham { get; set; }
        public string ValueRole_SanPham { get; set; }
        public string ValueRole_KhachHang { get; set; }
        public string ValueRole_ChiNhanh { get; set; }

        //Kinh doanh
        public string ValueRole_BaoGia { get; set; }
        public string ValueRole_HopDong { get; set; }
        public string ValueRole_TheoDoiLichHen { get; set; }
        public string ValueRole_NhatKyGiaoDich { get; set; }

        //Thống kê
        public string ValueRole_ThongkeDoanhThu { get; set; }
        public string ValueRole_TonKhoHienHanh { get; set; }

        //Bài viết - Video
        public string ValueRole_BaiViet { get; set; }
        public string ValueRole_Video { get; set; }
        public string ValueRole_AnhCongTrinh { get; set; }
        public string ValueRole_Catalogue { get; set; }
        public string ValueRole_HDSD { get; set; }
        public string ValueRole_KhoHinhAnh { get; set; }

        //QL công việc
        public string ValueRole_ThongKeCV { get; set; }
        public string ValueRole_DanhSachCV { get; set; }

        //QL nhân viên
        public string ValueRole_ThongTinCaNhan { get; set; }
        public string ValueRole_NgayNghi { get; set; }
        public string ValueRole_ChamCong { get; set; }
        public string ValueRole_ViTriNV { get; set; }

        //QL chung
        public string ValueRole_NguoiDung { get; set; }
        public string ValueRole_PQNguoiDung { get; set; }

        //QL phản ánh
        public string ValueRole_PhanAnhKH { get; set; }
        public string ValueRole_PhanAnhNV { get; set; }

        //QL thông báo
        public string ValueRole_LoaiThongBao { get; set; }
        public string ValueRole_NhomThongBao { get; set; }
        public string ValueRole_ThongBao { get; set; }

        public bool? LockEditProduct { get; set; }
    }
}
