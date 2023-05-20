using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_HopDong_ChiTiet")]
    public class WMS_HopDong_ChiTiet 
    {
        [Key]
        public int Id { get; set; }
        public int HopDong_Id { get; set; }
        [ForeignKey("HopDong_Id")]
        public virtual WMS_HopDong WMS_HopDong { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }
        public string ChatLuong { get; set; }
        public double? DonGia { get; set; }
        public double? SoLuong { get; set; }
        public double? ThanhTien { get; set; }
        public string GhiChu { get; set; }
        public int? Image_Id { get; set; }
        public int? ChatLuong_Id { get; set; }
        [ForeignKey("ChatLuong_Id")]
        public virtual FSW_ChatLuong FSW_ChatLuong { get; set; }
        public DateTime? NgayDaGiaoHang { get; set; }
        public int? OrderBy { get; set; }
    }
}
