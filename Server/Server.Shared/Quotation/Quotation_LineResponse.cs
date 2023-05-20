using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Quotation
{
    public class Quotation_LineResponse
    {
        public string Code { get; set; }
        public string Code2 { get; set; }
        public int Quotation_id { get; set; }
        public string Name { get; set; }
        public string Quality { get; set; }
        public string Specifications { get; set; }
        public string CountryOfOriginCode { get; set; }
        public bool? IsDelete { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemGroupName { get; set; }
        public string unitOfMeasureCode { get; set; }
        public string unitOfMeasureName { get; set; }
        public string LinkVideo { get; set; }



        public byte[] ImageByte { get; set; }
        public string Image { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public double? qty { get; set; }
        public double? price { get; set; }
        public double? amt { get; set; }
            
        public string ItemCode { get; set; }
        public byte[] AbsolutePath { get; set; }
        
        public double? HaNoi { get; set; } = 0;
        public double? HCM { get; set; } = 0;
        public double? BinhDuong { get; set; } = 0;
        public double? DongNai { get; set; } = 0;
        public double? QuyNhon { get; set; } = 0;
        public int? OrderBy { get; set; }
        public int? ChatLuongId { get; set; }
        public string ChatLuongName { get; set; }
        public List<string> QualityItem { get; set; }
    }
}
