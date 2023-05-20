using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Report
{
    public class ReportResponse
    {
        public int? SLCongViec { get; set; }
        public int? CV_HoanThanh { get; set; }
        public int? CV_ChuaHoanThanh { get; set; }
        public double? SoKm { get; set; } = 0;
        public int? SLChamCong { get; set; }
        public int? SLNgayNghi { get; set; }
        public int? SLDeXuat { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }
}
