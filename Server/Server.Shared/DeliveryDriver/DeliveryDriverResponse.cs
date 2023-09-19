using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.DeliveryDriver
{
    public class DeliveryDriverResponse
    {
        public int DeliveryDriverId { get; set; }
        public string CompleteName { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UploadImage { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
