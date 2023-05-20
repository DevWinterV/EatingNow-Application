using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_LoaiDanhGia")]
    public class WMS_LoaiDanhGia
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int? SoNgay_NhacNho { get; set; }
        public int? SoLan_NhacNho { get; set; }
        public string Description { get; set; }

    }
}
