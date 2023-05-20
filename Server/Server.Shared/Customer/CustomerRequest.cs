using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Customer
{
    public class CustomerRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? TinhThanh_Id { get; set; }
        public int TransactionTypeId { get; set; }
        public string EmployeeCode { get; set; }

        [JsonProperty("YearValue")]
        public int Year { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string PersonRepresent { get; set; }
        public string Position { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public string PersonContact { get; set; }
        public string ReciverAddress { get; set; }
        public string LienHeKhac { get; set; }
        public int? CustomerType_Id { get; set; }
        public string Tinh { get; set; }
        public int? UserId { get; set; }
        public string checkCustomerCode { get; set; }
    }
}
