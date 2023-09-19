using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.FoodList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CustomerDto
{
    public class EN_CustomerRequest : BaseRequest
    {
        public string CustomerId { get; set; }
        public string CompleteName { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Phone { get; set; }
        public string Payment { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public double TotalAmt { get; set; }
        public double TransportFee { get; set; }
        public double IntoMoney { get; set; }
        public int UserId { get; set; }
        public string TokenWeb { get; set; }
        public string TokenApp { get; set; }
        public List<EN_OrderLine> OrderLine { get; set; }
    }
}
