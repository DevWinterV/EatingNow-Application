using DaiPhucVinh.Server.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_Posts")]
    public class FUV_Posts
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        [ForeignKey("ImageId")]
        public virtual ImageRecord ImageRecord { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
