using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.TransactionInformation
{
    public class AppointmentTrackingResponse
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public int DanhGia_Id { get; set; }
        public string TenLoaiDanhGia { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? NgayChuyenTien { get; set; }
        public DateTime? NgayGiaoHang { get; set; }
        public string SoNgayDenHen { get; set; }
        public int DayRemain { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Description { get; set; }
        public string Appointment_Description { get; set; }

    }
}
