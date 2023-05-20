using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Location
{
    public class LocationResponse
    { 
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocationFullName { get; set; }
        public string PhoneNo { get; set; }
        public int City { get; set; }
        public string FaxNo { get; set; }
        public string ManagerName { get; set; }
        public string ManagerRole { get; set; }
        public string TaxCode { get; set; }
        public string BankingCode { get; set; }
        public string Address { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
    }
}
