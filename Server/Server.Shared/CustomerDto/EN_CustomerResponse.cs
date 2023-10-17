using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerDto
{
    public class EN_CustomerResponse
    {
        public string CustomerId { get; set; }
        public string CompleteName { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        public int WardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public string TokenWeb { get; set; }
        public string TokenApp { get; set; }
        public string ImageProfile { get; set; }

    }
}
