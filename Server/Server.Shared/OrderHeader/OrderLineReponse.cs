using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.OrderLineResponse
{
    public class OrderLineReponse
    {
        public int OrderLineId { get; set; }
        public string OrderHeaderId { get; set; }
        public int FoodListId { get; set; }
        public string CategoryName { get; set; }
        public string FoodName { get; set; }
        public int Price { get; set; }
        public int qty { get; set; }
        public string UploadImage { get; set; }
        public string Description { get; set; }
        public double? TotalPrice { get; set; }
    }
}
