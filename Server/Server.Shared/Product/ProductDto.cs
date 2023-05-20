using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Shared.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemCategoryCode { get; set; }
        public string Specifications { get; set; }
        public string Model { get; set; }
        public string CountryOfOriginCode { get; set; }
        public string Status { get; set; }
        public double? Inventory { get; set; }
        public string ImageRecordId { get; set; }
        public string MainImagePath { get; set; }
        public string PdfPath { get; set; }
        public string IdImage { get; set; }
        public DateTime? ImportDate { get; set; }
        public List<ImageRecord> ListImage { get; set; }
        public List<TaskMobileResponse> ListTask { get; set; }
    }
}
