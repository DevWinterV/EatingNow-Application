using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Server.Data.DaiPhucVinh
{
    [Table("WMS_Quotation")]
    public class WMS_Quotation
    {
        [Key]
        public int Id { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? Date { get; set; }
        public string CustomerCode { get; set; }
        [ForeignKey("CustomerCode")]
        public virtual WMS_Customers WMS_Customer { get; set; }
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public virtual WMS_Employee WMS_Employee { get; set; }
        public DateTime? Time { get; set; }
        public string Path { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string Note { get; set; }
        public string PaymentType { get; set; }
        public string WarrantyType { get; set; }
        public string WeliveryType { get; set; }
        public bool? IsSendEmail { get; set; }
        public double CommissionRate { get; set; }
        public double CommissionAmt { get; set; }
        public double Amt { get; set; }
        public double AmtLCY { get; set; }
        public double? VATRate { get; set; }
        public double? VAT { get; set; }
        public string LocationCode { get; set; }
        [ForeignKey("LocationCode")]
        public virtual WMS_Location WMS_Location { get; set; }
        public virtual ICollection<WMS_QuotationLine> WMS_QuotationLine { get; set; }
    }
}
