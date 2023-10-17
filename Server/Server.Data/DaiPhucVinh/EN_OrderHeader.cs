using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("OrderHeader")]
    public class EN_OrderHeader
    {
        [Key]
        public string OrderHeaderId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual EN_Customer EN_Customer { get; set; }
        public double TotalAmt { get; set; }
        public double TransportFee { get; set; }
        public double IntoMoney { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual EN_Store EN_Store { get; set; }
        public bool Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormatAddress { get; set; }
        public string NameAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }



    }
}
