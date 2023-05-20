using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_TransactionInformation")]
    public class WMS_TransactionInformation
    {
        [Key]
        public int Id { get; set; }
        public int TransactionType_Id { get; set; }
        [ForeignKey("TransactionType_Id")]
        public virtual WMS_TransactionType WMS_TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string ContacPerson { get; set; }
        public int Year { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerCode { get; set; }
        [ForeignKey("CustomerCode")]
        public virtual WMS_Customers WMS_Customer { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public virtual WMS_Employee WMS_Employee { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string FileAttach { get; set; }
        public string Subject { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Appointment_Description { get; set; }
        public bool? Appointment_IsSendEmail { get; set; }
        public string Appointment_IsSendEmail_MailTo { get; set; }
        public string Appointment_SendEmail_Description { get; set; }
        public string EmailManagerment { get; set; }
        public int? DanhGia_Id { get; set; }
        [ForeignKey("DanhGia_Id")]
        public virtual WMS_LoaiDanhGia WMS_LoaiDanhGia { get; set; }
        public bool? Is_DaLienHeLai_Lan1 { get; set; }
        public bool? Is_DaLienHeLai_Lan2 { get; set; } 
        public bool? Is_DaLienHeLai_Lan3 { get; set; }
        public bool? Is_DaGuiEmail_NhanVien { get; set; }
        public bool? Is_DaGuiEmail_QuanLy { get; set; }
        public bool? Is_DaGuiEmail_KhachHang { get; set; }
        public bool? Is_DaGuiSMS_NhanVien { get; set; }
        public bool? Is_DaGuiSMS_QuanLy { get; set; }
        public DateTime? NgayChuyenTien { get; set; }
        public DateTime? NgayGiaoHang { get; set; }
        public int? BaoGia_Id { get; set; }
        [ForeignKey("BaoGia_Id")]
        public virtual WMS_Quotation WMS_Quotation { get; set; }
        public int? HopDong_Id { get; set; }
        [ForeignKey("HopDong_Id")]
        public virtual WMS_HopDong WMS_HopDong { get; set; }

        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location  { get; set; }
    }
}
