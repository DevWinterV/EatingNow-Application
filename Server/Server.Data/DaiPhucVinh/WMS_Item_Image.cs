using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Item_Image")]
    public class WMS_Item_Image
    {
        [Key]
        public int Id { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public bool? IsMain { get; set; }
        public bool? IsHiden { get; set; }

    }
}
