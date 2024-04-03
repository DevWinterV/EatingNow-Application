using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Promotion")]
    public class EN_Promotion
    {
        [Key]
        public int PromotionID { get; set; }
        public string PromoCode { get; set; }
        public string PromoName { get; set; }
        public string PromoDetail { get; set; }
        public string DiscountType { get; set; }
        public int DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool PromoCStatus { get; set; }
        public int UsageCount { get; set; }
        public int MaxUsageLimit { get; set; }
        public bool IsPromoystem { get; set; }
        public int StoreId { get; set; }
        public int FoodListId { get; set; }
    }
}
