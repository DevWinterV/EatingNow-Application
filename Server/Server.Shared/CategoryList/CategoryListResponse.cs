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
        public string Address {  get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
