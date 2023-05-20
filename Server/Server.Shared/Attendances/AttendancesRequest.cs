using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Attendances
{
    public class AttendancesRequest: BaseRequest
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string CheckInData { get; set; }
        public double? LongCheckIn { get; set; }
        public double? LatCheckIn { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string CheckOutData { get; set; }
        public double? LongCheckOut { get; set; }
        public double? LatCheckOut { get; set; }
        public string Note { get; set; }
        public bool Accept { get; set; }
        public string NoteReply { get; set; }
        public string StatusCode { get; set; }
        public string LocationImagesCheckin { get; set; }
        public string LocationImagesCheckout { get; set; }
    }
}
