using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.FoodList
{
    public class FoodListFillterRequest
    {
        public int Id { get; set; } 
        public int Qtycontrolled { get; set; }
        public int QuantitySupplied { get; set; }
        public int ExpiryDate { get ; set; }    
        public int TimeExpiryDate { get; set; }
        public string keyWord { get; set; }
    }
}
