using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FoodList")]
    public class EN_FoodList
    {
        [Key]
        public int FoodListId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual EN_CategoryList Category { get; set; }
        public string FoodName { get; set; }
        public int Price { get; set; }
        public int qty { get; set; }
        public string UploadImage { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public int? Hint { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsNoiBat { get; set; }
    }
}
