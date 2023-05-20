using System;

namespace DaiPhucVinh.Shared.Excel
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PartnerCode { get; set; }
        public string LocationCode { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string CardNumber { get; set; }
        public string IDCard { get; set; }
        public DateTime? BirthDay { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string CustomerGroup_Code { get; set; }
        public string CustomerType_Code { get; set; }
        public double? ConNoPhaiThu { get; set; }
        public double? DiemHienTai { get; set; }
        public double? DiemTichLuu { get; set; }
        public int? ChuKyThanhToan { get; set; }
        public string NumberBankAccount { get; set; }
        public string BankName { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
