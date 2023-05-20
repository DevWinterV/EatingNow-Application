using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CategoryList
{
    public class CategoryListResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string NameStore { get; set; }
        public string DescriptionStore { get; set; }
        public string OpenTime { get; set; }
        public bool Status { get; set; }
    }
}
