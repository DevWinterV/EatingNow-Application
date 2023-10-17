using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.OrderHeader
{
    public class OrderLineRequest
    {
        public int OrderLineId { get; set; }
        public string OrderHeaderId { get; set; }
        public int FoodListId { get; set; }
    }
}
