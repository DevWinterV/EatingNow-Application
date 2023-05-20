using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_ItemCategory")]
    public class WMS_ItemCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
        public int Id { get; set; }
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemGroupCode { get; set; }
        public string CostMethodCode { get; set; }
        public string VATGroupCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
