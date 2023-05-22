using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Store
{
    public class FilterStoreByCusineRequest
    {
        public int CuisineId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int Count { get; set; } = 5;
    }
}
