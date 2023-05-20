using DaiPhucVinh.Server.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Task
{
    public class TaskMobileResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int? AssignUserId { get; set; }
        public string AssignUserName { get; set; }
        public string TaskTypeName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerWorkAddress { get; set; }
        public float? Long { get; set; }
        public float? Lat { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? TimeToStart { get; set; }
        public DateTime? TimeToLocation { get; set; }
        public bool? CheckToLocation { get; set; }
        public string Note_Machine { get; set; } 
        public string Note_Suggest { get; set; }
        public string Note_Result { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatusName { get; set; }
        public int? TaskResultId { get; set; }
        public string TaskResultName { get; set; }
        public DateTime? TaskFinishAt { get; set; }
        public double? RatingPoint { get; set; }
        public string RatingComment { get; set; }
        public string Distance { get; set; }
        public List<ImageRecord> ListBeforeImage { get; set; }
        public List<ImageRecord> ListAfterImage { get; set; }
        public List<ImageRecord> ListDocumentImage { get; set; }    }
}
