using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.FoodList
{
    public class FoodListResponse
    {
        public int FoodListId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string FoodName { get; set; }
        public int Price { get; set; }
        public int qty { get; set; }
        public string UploadImage { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
    }
}
