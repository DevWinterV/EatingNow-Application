using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Store
{
    public  class ListOfProductSold 
    {
       public int FoodListId { get; set; }
       public string UserId { get; set; }
       public string FoodName { get; set; }
       public int FoodCount { get; set; }   
    }
}
