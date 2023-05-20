using DaiPhucVinh.Shared.Common;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Catalogue
{
    public class CatalogueRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string ImageName { get; set; }
        public string PdfFile { get; set; }
        public string PdfFileName { get; set; }
        public string Model { get; set; }
    }
}