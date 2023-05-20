using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Code2 { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        /// <summary>
        /// Thông số kỹ thuật
        /// </summary>
        public string Specifications { get; set; }
        /// <summary>
        /// Xuất xứ
        /// </summary>
        public string CountryOfOriginCode { get; set; }
        public double? UnitPrice { get; set; }
        public string Model { get; set; }
        public string ItemGroup_Code { get; set; }
        public string ItemGroup_Name { get; set; }
        public string ImageRecordId { get; set; }
        public string Description { get; set; }
        public double? UnitCost_Whole { get; set; }
        public bool? IsDelete { get; set; }
        public bool check { get; set; }
        public double? qty { get; set; }
        public double? price { get; set; }
        public double? amt { get; set; }
        public byte[] AbsolutePath { get; set; }
        public string LinkVideo { get; set; }
        public string CodeItemCategory { get; set; }
        public string NameItemCategory { get; set; }
        public int? OrderBy { get; set; }
        public int? ChatLuongId { get; set; }

    }
}
