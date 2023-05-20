using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Product
{
    public class ProductByCustomerDto
    {
        public DateTime? NgayKy { get; set; }
        public string SoHopDong { get; set; }
        public string Id { get; set; }
        public List<ProductDto> ListProduct { get; set; }
    }
}
