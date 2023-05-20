using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_QuotationLine")]
    public class WMS_QuotationLine
    {
        [Key]
        public int Id { get; set; }
        public int Quotation_Id { get; set; }
        [ForeignKey("Quotation_Id")]
        public virtual WMS_Quotation WMS_Quotation { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }
        public string LotNo { get; set; }
        public string SeriesNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public double? UnitPrice { get; set; }
        public double? Qty { get; set; }
        public double? SubAmt { get; set; }
        public double? DiscountRate { get; set; }
        public double? DiscountAmt { get; set; }
        public double? CommissionRate { get; set; }
        public double? CommissionAmt { get; set; }
        public double? VatRate { get; set; }
        public double? VatAmt { get; set; }
        public double? Amt { get; set; }
        public double? AmtLCY { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Quality { get; set; }
        public int? Image_Id { get; set; }
        public string ImageId_List { get; set; }
        public int? ChatLuong_Id { get; set; }
        [ForeignKey("ChatLuong_Id")]
        public virtual FSW_ChatLuong FSW_ChatLuong { get; set; }
        public string HinhList { get; set; }
        public int? OrderBy { get; set; }
    }
}
