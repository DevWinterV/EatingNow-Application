using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("paymentonline")]
    public class EN_Paymentonline
    {
        [Key]
        public int idPaymentonline {  get; set; } 
	    public int userId {  get; set; }    
        [ForeignKey("userId")]
        public virtual EN_Store Store { get; set; }
        public int CategoryPaymentId { get; set; }
        [ForeignKey("CategoryPaymentId")]
        public virtual EN_CategoryPayment CategoryPayment { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }

    }
}
