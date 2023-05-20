using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Item")]
    public class WMS_Item
    {
        public int Id { get; set; }
        [Key]
        public string Code { get; set; }
        public string Code2 { get; set; } 
        public string Name { get; set; }
        public string Name_Unsigned { get; set; }
        public string UnitOfMeasure { get; set; }
        [ForeignKey("UnitOfMeasure")]
        public virtual WMS_UnitOfMeasure WMS_UnitOfMeasure { get; set; }
        public string ItemCategoryCode { get; set; }
        [ForeignKey("ItemCategoryCode")]
        public virtual WMS_ItemCategory WMS_ItemCategory { get; set; }
        public string ItemPostingGroupCode { get; set; }
        public string VATGroupCode { get; set; }
        public string LotNo { get; set; }
        public string SeriesNo { get; set; }
        public string ItemSize { get; set; }
        public string ItemColor { get; set; }
        public string Specifications { get; set; }
        public string BarCode { get; set; }
        public string ManufactureCode { get; set; }
        public string CountryOfOriginCode { get; set; }
        public string ItemGroup1 { get; set; }
        public string ItemGroup2 { get; set; }
        public string CostMethodCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost { get; set; }
        public double? UnitCost_Whole { get; set; }
        public double? StandardCost { get; set; }
        public double? MaximunInventory { get; set; }
        public double? MinimunInventory { get; set; }
        public double? GrossWeight { get; set; }
        public double? NetWeight { get; set; }
        public double? DimensionValue { get; set; }
        public string VendorCode { get; set; }
        public string Picture { get; set; }
        public bool? AllowNegativeInventory { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public double? UserDefine_1 { get; set; }
        public double? UserDefine_2 { get; set; }
        public double? UserDefine_3 { get; set; }
        public double? UserDefine_4 { get; set; }
        public double? UserDefine_5 { get; set; }
        public double? UserDefine_6 { get; set; }
        public bool? IsChecked { get; set; }
        public string CustomerCode { get; set; }
        public byte[] Image { get; set; }
        public string ImageRecordId { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }
        public string ItemGroup_Code { get; set; }
        [ForeignKey("ItemGroup_Code")]
        public virtual WMS_ItemGroup WMS_ItemGroup { get; set; }
        public string LinkVideo { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsNew { get; set; }
    }
}
