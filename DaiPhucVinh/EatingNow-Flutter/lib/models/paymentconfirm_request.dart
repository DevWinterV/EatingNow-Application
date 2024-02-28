import 'OrderRequest.dart';

class PaymentTransaction {
  String vnpAmount;
  String vnpBankCode;
  String vnpBankTranNo;
  String vnpCardType;
  String vnpOrderInfo;
  String vnpPayDate;
  String vnpResponseCode;
  String vnpSecureHash;
  String vnpTmnCode;
  String vnpTransactionNo;
  String vnpTransactionStatus;
  String vnpTxnRef;
  OrderRequest requestOrder;

  PaymentTransaction({
    required this.vnpAmount,
    required this.vnpBankCode,
    required this.vnpBankTranNo,
    required this.vnpCardType,
    required this.vnpOrderInfo,
    required this.vnpPayDate,
    required this.vnpResponseCode,
    required this.vnpSecureHash,
    required this.vnpTmnCode,
    required this.vnpTransactionNo,
    required this.vnpTransactionStatus,
    required this.vnpTxnRef,
    required this.requestOrder,
  });

  factory PaymentTransaction.fromJson(Map<String, dynamic> json) {
    return PaymentTransaction(
      vnpAmount: json['vnp_Amount'],
      vnpBankCode: json['vnp_BankCode'],
      vnpBankTranNo: json['vnp_BankTranNo'],
      vnpCardType: json['vnp_CardType'],
      vnpOrderInfo: json['vnp_OrderInfo'],
      vnpPayDate: json['vnp_PayDate'],
      vnpResponseCode: json['vnp_ResponseCode'],
      vnpSecureHash: json['vnp_SecureHash'],
      vnpTmnCode: json['vnp_TmnCode'],
      vnpTransactionNo: json['vnp_TransactionNo'],
      vnpTransactionStatus: json['vnp_TransactionStatus'],
      vnpTxnRef: json['vnp_TxnRef'],
      requestOrder: OrderRequest.fromJson(json['requestOrder']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'vnp_Amount': vnpAmount,
      'vnp_BankCode': vnpBankCode,
      'vnp_BankTranNo': vnpBankTranNo,
      'vnp_CardType': vnpCardType,
      'vnp_OrderInfo': vnpOrderInfo,
      'vnp_PayDate': vnpPayDate,
      'vnp_ResponseCode': vnpResponseCode,
      'vnp_SecureHash': vnpSecureHash,
      'vnp_TmnCode': vnpTmnCode,
      'vnp_TransactionNo': vnpTransactionNo,
      'vnp_TransactionStatus': vnpTransactionStatus,
      'vnp_TxnRef': vnpTxnRef,
      'requestOrder': requestOrder.toJson(),
    };
  }
}
