using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Customers")]
    public class WMS_Customers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string Name { get; set; }
        public string Name_Unsigned { get; set; }
        public string SearchName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public int? ImageRecordId { get; set; }
        [ForeignKey("ImageRecordId")]
        public virtual Entity.ImageRecord ImageRecord { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string EmployeeCode { get; set; }

        [ForeignKey("EmployeeCode")]
        public virtual WMS_Employee WMS_Employee { get; set; }
        public string PersonContact { get; set; }
        public string PersonEpresent { get; set; }
        public string Position { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string Bank { get; set; }
        public int? TinhThanh_Id { get; set; }
        [ForeignKey("TinhThanh_Id")]
        public virtual WMS_Province WMS_Province { get; set; }
        public int? Country_Id { get; set; }
        public int? CustomerType_Id { get; set; }
        [ForeignKey("CustomerType_Id")]
        public virtual WMS_CustomersType WMS_CustomersType { get; set; }
        public string Tinh { get; set; }
        public string QuocGia { get; set; }
        public string LoaiKhachHang { get; set; }
        public string NhanVienKD { get; set; }
        public string ReciverAddress { get; set; }
        public string LienHeKhac { get; set; }
        public int? UserId { get; set; }
    }
}
