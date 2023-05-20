using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("OrderLine")]
    public class EN_OrderLine
    {
        [Key]
        public int OrderLineId { get; set; }
        public string OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual EN_OrderHeader EN_OrderHeader { get; set; }
        public int FoodListId { get; set; }
        public int CategoryId { get; set; }
        public string FoodName { get; set; }
        public int Price { get; set; }
        public int qty { get; set; }
        public string UploadImage { get; set; }
        public string Description { get; set; }
    }
}
