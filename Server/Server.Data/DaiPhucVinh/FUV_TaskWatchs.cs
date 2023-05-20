using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_TaskWatchs")]
    public class FUV_TaskWatchs
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual FUV_Tasks FUV_Tasks { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Server.Data.Entity.User User { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
