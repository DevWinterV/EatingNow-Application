using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Attendances
{
    public class ExportAttendancesResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime StartTime { get; set; }
        public string CheckInData { get; set; }
        public string CheckOutData { get; set; }
        public DateTime CheckOutTime { get; set; }
        public string Note { get; set; }
        public bool Accept { get; set; }
        public string NoteReply { get; set; }
    }
}
