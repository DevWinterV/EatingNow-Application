using DaiPhucVinh.Server.Data.DaiPhucVinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerDto
{
    public  class EN_CustomerAddressResponse
    {
        public int AddressId { get; set; }
        public string CustomerId { get; set; }
        public string Name_Address { get; set; }
        public string Format_Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string CustomerName { get; set; }
        public string PhoneCustomer { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public int WardId { get; set; }
        public bool Defaut { get; set; }
    }
}
