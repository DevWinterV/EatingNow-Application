using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("CategoryPayment")]
    public class EN_CategoryPayment
    {
        [Key]
        public int id { get; set; }
        public string name {get; set;}
    }
}
