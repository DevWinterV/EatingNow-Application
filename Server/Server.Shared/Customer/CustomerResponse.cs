using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Customer
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string FaxNo { get; set; }
        public string PersonContact { get; set; }
        public string PersonEpresent { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public int TinhThanh_Id { get; set; }
        public string Tinh { get; set; }
        public string Position { get; set; }
        public int CustomerType_Id { get; set; }
        public string CustomerTypeName { get; set; }
        public  string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ReciverAddress { get; set; }
        public string LienHeKhac { get; set; }
        public int? UserId { get; set; }
        public bool IsExistAccount { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string ImageRecordId { get; set; }
        public List<string> Avatar { get; set; }
    }
}
