using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Store
{
    public class StatisticalResponse
    {
        public double? revenueDate { get; set; }
        public double? revenueWeek { get; set; }
        public double? revenueMonth { get; set; }
        public double? revenueYear { get; set; }
        public List<StatisticalChart> listChart { get; set; }

    }
}
