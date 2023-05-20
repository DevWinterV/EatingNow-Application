using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_TransactionType")]
    public class WMS_TransactionType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }



    }
}
