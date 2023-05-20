using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Task
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }
        public int? AssignUserId { get; set; }
        public string AssignUserName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerWorkAddress { get; set; }
        public string CustomerPhoneDefault { get; set; }
        public string CustomerPhone { get; set; }
        public string Distance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public DateTime? TimeToStart { get; set; }
        public DateTime? TimeToLocation { get; set; }
        public string LocationImages { get; set; }
        public List<string> LocationImageList { get; set; }
        public string Note_Machine { get; set; }
        public string BeforeProcessImages { get; set; }
        public List<string> BeforeProcessImageList { get; set; }
        public string Note_Suggest { get; set; }
        public string AfterProcessImages { get; set; }
        public List<string> AfterProcessImageList { get; set; }
        public string Note_Result { get; set; }
        public string DocumentImages { get; set; }
        public List<string> DocumentImageList { get; set; }
        public string ESignature { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatusCode { get; set; }
        public string TaskStatusName { get; set; }
        public int TaskResultId { get; set; }
        public string TaskResultName { get; set; }
        public DateTime? TaskFinishAt { get; set; }
        public double? RatingPoint { get; set; }
        public string RatingComment { get; set; }
        public string TaskStatusClassName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDisplayName { get; set; }
        public bool IsLate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

    }
}
