using DaiPhucVinh.Shared.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Task
{
    public class TaskRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStatusId { get; set; }
        public int? AssignUserId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerWorkAddress { get; set; }
        public string CustomerPhoneDefault { get; set; }
        public string CustomerPhone { get; set; }
        public string Distance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string LocationCode { get; set; }
        public string DocumentImages { get; set; }

        public double? Lng { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public DateTime? TimeToStart { get; set; }
        public DateTime? TimeToLocation { get; set; }

        public string ItemCode { get; set; }
        public string Note_Machine { get; set; }
        public string Note_Suggest { get; set; }

        public int TaskResultId { get; set; }
        public string Note_Result { get; set; }
        public DateTime? TaskFinishAt { get; set; }
        public double? RatingPoint { get; set; }
        public string RatingComment { get; set; }


    }
}
