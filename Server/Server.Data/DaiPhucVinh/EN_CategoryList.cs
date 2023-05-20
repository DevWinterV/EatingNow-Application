using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("CategoryList")]
    public class EN_CategoryList
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual EN_Store Store { get; set; }
        public bool Status { get; set; }
    }
}
