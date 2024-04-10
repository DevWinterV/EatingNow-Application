using DaiPhucVinh.Server.Data.DaiPhucVinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.FoodList
{
    public class FoodList_Store
    {
        public EN_FoodList foodItem {  get; set; }  
        public EN_Store Storeitem { get; set; }
        public int foodItemQtyAvailable { get; set; }   
    }
}
