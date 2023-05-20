using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("Cuisine")]
    public class EN_Cuisine
    {
        [Key]
        public int CuisineId { get; set; }
        public string AbsoluteImage { get; set; }
        public string Name { get; set; }
    }
}
