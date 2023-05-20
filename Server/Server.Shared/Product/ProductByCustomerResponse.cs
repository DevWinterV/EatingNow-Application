using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Shared.HopDong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Product
{
    public class ProductByCustomerResponse
    {
        public string SoHopDong { get; set; }
        public string Date { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public List<ProductByCustomerDto> Items { get; set; }
    }
}
