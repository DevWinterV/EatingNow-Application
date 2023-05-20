using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.UserGuides
{
    public class UserGuidesRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public string PdfFile { get; set; }
        public string Images { get; set; }
        public string ItemCode { get; set; }
    }
}
