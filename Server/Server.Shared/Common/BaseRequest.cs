using System;

namespace DaiPhucVinh.Shared.Common
{
    public class BaseRequest
    {
        public string Term { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }
        public int? ItemCategoryCode { get; set; }
       public string ProvinceName {  get; set; }
        public int ProvinceId { get; set; }
       public string   EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
        public int OrderType { get; set; }  
    }
}
