using DaiPhucVinh.Shared.Product;
using System;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Catalogue
{
    public class CatalogueResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public string PdfFile { get; set; }
        public int PdfId { get; set; }
        public string PdfFileAbsolutePath { get; set; }
        public string PdfFileName { get; set; }
        public string Images { get; set; }
        public string ImageAbsolutePath { get; set; }
        public int ImageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<ProductDto> ListProducts { get; set; }
    }
}
