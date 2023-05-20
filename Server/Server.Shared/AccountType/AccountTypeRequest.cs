using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Shared.AccountType
{
    public class AccountTypeRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
