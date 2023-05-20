using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Attendances
{
    public class AttendancesResponse
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public float? Long { get; set; }
        public float? Lat { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string CheckInData { get; set; }
        public string LocationImagesCheckin { get; set; }
        public List<string> ImagesCheckin { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string CheckOutData { get; set; }
        public string LocationImagesCheckout { get; set; }
        public List<string> ImagesCheckout { get; set; }
        public string Note { get; set; }
        public bool Accept { get; set; }
        public string NoteReply { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? SubtractTimeCheckin { get; set; }
       public List<string> ListImageCheckIn { get; set; }
        public List<string> ListImageCheckOut { get; set; }
    }
}
