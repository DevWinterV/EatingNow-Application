using DaiPhucVinh.Shared.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.CategoryItem
{
    public class CategoryItemResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ProductDto> ListProduct { get; set; }
    }
}
