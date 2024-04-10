using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Store
{
    public class StoreRequest : BaseRequest
    {
        public int UserId { get; set; }
        public string AbsoluteImage { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string OpenTime { get; set; }
        public int ProvinceId { get; set; }
        public int CuisineId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Status { get; set; }
        public TimeSpan TimeOpen { get; set; }
        public TimeSpan TimeClose { get; set; }
    }
}
