using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Shared.TransactionHistoryTransactionInformation
{
    public class TransactionHistoryTypeRequest : BaseRequest
    {
        public int ProvinceId { get; set; }
        public int Year { get; set; }
        public int TransactionTypeId { get; set; }
        public string EmployeeCode { get; set; }
    }
}
