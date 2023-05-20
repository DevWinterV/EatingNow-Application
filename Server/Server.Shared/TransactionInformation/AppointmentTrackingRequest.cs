using DaiPhucVinh.Shared.Common;
using System;

namespace DaiPhucVinh.Shared.TransactionInformation
{
    public class AppointmentTrackingRequest : BaseRequest
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
    }
}
