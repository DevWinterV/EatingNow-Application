using DaiPhucVinh.Shared.ImageRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.inventoryitem
{
   public class InventoryitemResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public double? UnitPrice { get; set; }
        public double Qty { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ItemGroup { get; set; }
        public byte[] AbsolutePath { get; set; }
        public string ItemCode { get; set; }
        public double? HaNoi { get; set; } = 0;
        public double? HCM { get; set; } = 0;
        public double? BinhDuong { get; set; } = 0;
        public double? DongNai { get; set; } = 0;
        public double? QuyNhon { get; set; } = 0;
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }
}
