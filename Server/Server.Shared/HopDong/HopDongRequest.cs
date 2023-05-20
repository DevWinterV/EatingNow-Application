using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.HopDong
{
    public class HopDongRequest : BaseRequest
    {
        public int Id { get; set; }
        public string KhachHang_Code { get; set; }
        public string ContractNumber { get; set; }
        public DateTime NgayTaoHopDong { get; set; }
        public DateTime? NgayKy { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int? QuatationCode { get; set; }
        public string QuatationName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? CityCode { get; set; }
        public string ItemGroup_Code { get; set; }

        public string BenA_Name { get; set; }
        public string BenA_Address { get; set; }
        public string BenA_Phone { get; set; }
        public string BenA_Fax { get; set; }
        public string BenA_MST { get; set; }
        public string BenA_Banking { get; set; }
        public string BenA_ManagerName { get; set; }
        public string BenA_ManagerRole { get; set; }

        public string BenB_Name { get; set; }
        public string BenB_Address { get; set; }
        public string BenB_Phone { get; set; }
        public string BenB_Fax { get; set; }
        public string BenB_MST { get; set; }
        public string BenB_Banking { get; set; }
        public string BenB_ManagerName { get; set; }
        public string BenB_ManagerRole { get; set; }

        public double TongTien { get; set; }
        public double Thue { get; set; }
        public double PhanTramThue { get; set; }
        public double TienSauThue { get; set; }

        public DateTime? NgayChuyenTien { get; set; }
        public double ChuyenTienDot1 { get; set; }
        public double ChuyenTienDot2 { get; set; }
        public double ChuyenTienDot3 { get; set; }
        public DateTime? NgayGiaoHang { get; set; }

        public string Dieu2 { get; set; }
        public string Dieu3 { get; set; }
        public string Dieu4 { get; set; }
        public string Dieu5 { get; set; }
        public string Dieu6 { get; set; }


        public List<ProductResponse> dataItemCheck { get; set; }
    }
}
