using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerDto
{
    public class EN_CustomerAddressRequest
    {
        public int AddressId { get; set; }
        public string Name_Address { get; set; }
        public string Format_Address { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneCustomer { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public bool Defaut { get; set; }
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public int DistrictId { get; set; }
    }
}
