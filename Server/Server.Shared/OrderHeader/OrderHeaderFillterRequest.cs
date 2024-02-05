using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.OrderHeader
{
    public class OrderHeaderFillterRequest
    {
        public int Id { get; set; }
        public int OrderStatus { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string keyword { get; set; }

    }
}
