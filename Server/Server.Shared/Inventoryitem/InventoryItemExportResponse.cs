using DaiPhucVinh.Server.Data.DaiPhucVinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Inventoryitem
{
    public class InventoryItemExportResponse
    {
        //public string ItemCategoryCode { get; set; }
        //public string ItemCategoryName { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public double? TotalInventory { get; set; } //tổng tồn
        public double? HaNoi { get; set; }
        public double? HCM { get; set; }
        public double? BinhDuong { get; set; }
        public double? DongNai { get; set; }
        public double? QuyNhon { get; set; }

    }
}
