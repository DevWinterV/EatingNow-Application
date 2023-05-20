using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.TransactionInformation
{
    public class TransactionInformationResponse
    {
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public DateTime Date { get; set; }
        public string ContacPerson { get; set; }
        public int Year { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string Appointment_Description { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DanhGia_Id { get; set; }
        public int BaoGia_Id { get; set; }
        public int HopDong_Id { get; set; }
        public string SoBaoGia { get; set; }
        public string SoHopDong { get; set; }
        public string TenLoaiDanhGia { get; set; }
        public bool Is_DaLienHeLai_Lan1 { get; set; }

    }
}
