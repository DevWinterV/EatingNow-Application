using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_Tasks")]
    public class FUV_Tasks
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        [ForeignKey("TaskTypeId")]
        public virtual FUV_TaskTypes FUV_TaskTypes { get; set; }
        public int? AssignUserId { get; set; }
        [ForeignKey("AssignUserId")]
        public virtual Server.Data.Entity.User User { get; set; }
        public string CustomerCode { get; set; }
        [ForeignKey("CustomerCode")]
        public virtual WMS_Customers WMS_Customer { get; set; }
        public string CustomerWorkAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string Distance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public DateTime? TimeToStart { get; set; }
        public DateTime? TimeToLocation { get; set; }
        public bool? CheckToLocation { get; set; }
        public string LocationImages { get; set; }

        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }        public string Note_Machine { get; set; }
        public string BeforeProcessImages { get; set; }
        public string Note_Suggest { get; set; }
        public string AfterProcessImages { get; set; }
        public string Note_Result { get; set; }
        public string DocumentImages { get; set; }
        public string ESignature { get; set; }

        public int TaskStatusId { get; set; }
        [ForeignKey("TaskStatusId")]
        public virtual FUV_TaskStatus FUV_TaskStatus { get; set; }
        public int? TaskResultId { get; set; }
        [ForeignKey("TaskResultId")]
        public virtual FUV_TaskResult FUV_TaskResult { get; set; }
        public DateTime? TaskFinishAt { get; set; }
        public double? RatingPoint { get; set; }
        public string RatingComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDone { get; set; }
        public bool Deleted { get; set; }
    }
}
