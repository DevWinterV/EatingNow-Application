using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.UserGuides
{
    public class UserGuidesResponse
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
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Model { get; set; }
    }
}
