using DaiPhucVinh.Shared.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Payment
{
    public class PaymentConfirmRequest
    {
        public string Vnp_Amount { get; set; }
        public string Vnp_BankCode { get; set; }
        public string Vnp_BankTranNo { get; set; }
        public string Vnp_CardType { get; set; }
        public string Vnp_OrderInfo { get; set; }
        public string Vnp_PayDate { get; set; }
        public string Vnp_ResponseCode { get; set; }
        public string Vnp_TmnCode { get; set; }
        public string Vnp_TransactionNo { get; set; }
        public string Vnp_TransactionStatus { get; set; }
        public string Vnp_TxnRef { get; set; }
        public string Vnp_SecureHash { get; set; }
        public EN_CustomerRequest requestOrder { get;set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "vnp_Amount", this.Vnp_Amount },
                { "vnp_BankCode", this.Vnp_BankCode },
                { "vnp_BankTranNo", this.Vnp_BankTranNo },
                { "vnp_CardType", this.Vnp_CardType },
                { "vnp_OrderInfo", this.Vnp_OrderInfo },
                { "vnp_PayDate", this.Vnp_PayDate },
                { "vnp_ResponseCode", this.Vnp_ResponseCode },
                { "vnp_TmnCode", this.Vnp_TmnCode },
                { "vnp_TransactionNo", this.Vnp_TransactionNo },
                { "vnp_TransactionStatus", this.Vnp_TransactionStatus },
                { "vnp_TxnRef", this.Vnp_TxnRef },
                { "vnp_SecureHash", this.Vnp_SecureHash },
                // ... add other properties as needed ...
            };

            return dictionary;
        }
    }

}
