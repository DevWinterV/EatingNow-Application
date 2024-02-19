using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.OrderHeaderResponse
{
    public class OrderHeaderResponse
    {
        public string OrderHeaderId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CustomerId { get; set; }
        public string TokenWeb { get; set; }
        public string TokenApp { get; set; }
        public double TotalAmt { get; set; }
        public double TransportFee { get; set; }
        public double IntoMoney { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string FormatAddress { get; set; }
        public string NameAddress { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PaymentStatusID { get; set; }   
    }
}
