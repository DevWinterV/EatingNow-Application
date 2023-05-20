using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Chart
{
    public class RevenueResponse
    {
        public string NgayKy { get; set; }
        public string Thang { get; set; }
        public double? TongTien { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int City_Id { get; set; }
        public string City_Name { get; set; }

        public List<ItemResponse> Item { get; set; }
    }
}
