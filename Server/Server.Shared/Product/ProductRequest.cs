using DaiPhucVinh.Shared.Common;
using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Product
{
    public class ProductRequest : BaseRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Code2 { get; set; }
        public string Model { get; set; }
        public string CountryOfOriginCode { get; set; }
        public string Description { get; set; }
        public bool? IsDelete { get; set; }
        public string Specifications { get; set; }
        public string ItemGroupCode { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? UnitCost_Whole { get; set; }

        public string ItemCategoryCode { get; set; }
        public string ItemCategoryName { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UnitOfMeasureName { get; set; }
        public string LinkVideo { get; set; }
        


        public string ItemCode { get; set; }
        public string ItemCatalogueCode { get; set; }
        public string ItemGroup_Code { get; set; }
        public List<QuestionProductRequest> ProductItem { get; set; }
        public List<string> dataItemCode { get; set; }

    }
}
