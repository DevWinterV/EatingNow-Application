using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("FUV_NotificationGroups")]
    public class FUV_NotificationGroups
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string UserIds { get; set; }
    }
}
