using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_InventoryItem")]
    public  class WMS_InventoryItem
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? DocumentType { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual WMS_Item WMS_Item { get; set; }
        public string UnitOfMeasure { get; set; }
        public double? NetWeight { get; set; }
        public double? DimValue { get; set; }
        public string LotNo { get; set; }
        public string SeriesNo { get; set; }
        public string BarCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CurrencyCode { get; set; }
        public double? ExchangeRate { get; set; }
        public double? UnitPrice { get; set; }
        public double? Qty { get; set; }
        public double? TotalNetWeight { get; set; }
        public double? TotalDimValue { get; set; }
        public double? QtyOfItemOnPallet { get; set; }
        public int? CellType { get; set; }
        public string CellCode { get; set; }
        public double? SubAmt { get; set; }
        public double? DiscountRate { get; set; }
        public double? DiscountAmt { get; set; }
        public double? CommissionRate { get; set; }
        public double? CommissionAmt { get; set; }
        public double? VatRate { get; set; }
        public double? VatAmt { get; set; }
        public double? Amt { get; set; }
        public double? AmtLCY { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public string ContractNo { get; set; }
        public string CustomsDeclaration { get; set; }
        public string CustomerCode { get; set; }
        public string VendorCode { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public string BranchCode { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }

    }
}
