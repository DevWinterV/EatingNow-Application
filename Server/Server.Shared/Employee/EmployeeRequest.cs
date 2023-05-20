using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Employee
{
    public class EmployeeRequest : BaseRequest
    {
        public string EmployeeCode { get; set; }
        public string LocationCode { get; set; }
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }
}
