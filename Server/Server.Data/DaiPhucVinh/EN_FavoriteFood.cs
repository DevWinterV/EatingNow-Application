using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FavoriteFoods")]

    public class EN_FavoriteFoods
    {
        [Key]
        public int FavoriteID { get; set; }
        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual EN_Customer EN_Customer { get; set; }
        public int FoodID { get; set; }
        [ForeignKey("FoodID")]
        public virtual EN_FoodList EN_FoodList { get; set; }
        public DateTime AddedAt { get; set; }

    }
}
