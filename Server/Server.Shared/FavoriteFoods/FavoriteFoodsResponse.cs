using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.FavoriteFoods
{
    public class FavoriteFoodsResponse 
    {
        public int FavoriteID { get; set; }
        public string CustomerID { get; set; }
        public int FoodID { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
