using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.OrderLineReponse
{
    public class OrderHeaderRequest : BaseRequest
    {
        public string OrderHeaderId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomerId { get; set; }
        public double TotalAmt { get; set; }
        public double TransportFee { get; set; }
        public double IntoMoney { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }

    }
}
