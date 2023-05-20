using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.Dayoffs
{
    public class DayoffsRequest : BaseRequest
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int DayRequests { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public int DayApproved { get; set; }
        public string ApprovedDescription { get; set; }
        public bool Approved { get; set; }
        public string LocationCode { get; set; }
        public bool Deleted { get; set; } //using for mobile app
        public DateTime CreatedAt { get; set; }
    }
}
