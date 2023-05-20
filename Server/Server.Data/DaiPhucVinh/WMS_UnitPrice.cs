using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_UnitPrice")]
    public class WMS_UnitPrice
    {
        [Key]
        public int Id { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual WMS_Employee WMS_Employee { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost_Whole { get; set; }
        public double? UnitCost_Retail { get; set; }
    }
}
