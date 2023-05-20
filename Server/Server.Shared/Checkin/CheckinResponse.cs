using System;


namespace DaiPhucVinh.Shared.Checkin
{
    public class CheckinResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public DateTime TimeCheckin { get; set; }
    }
}
