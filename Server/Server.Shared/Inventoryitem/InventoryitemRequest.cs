using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.inventoryitem
{
    public class InventoryitemRequest : BaseRequest
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ItemCode { get; set; }
        public string ItemGroupCode { get; set; }
        public string ItemName { get; set; }
        public string ItemModel { get; set; }
        public double Qty { get; set; }
        public string ImageRecordId { get; set; }
        public List<int> dataItemCode { get; set; }
        public List<string> InDataImage { get; set; }
        public int typeSearch { get; set; }
    }
}
