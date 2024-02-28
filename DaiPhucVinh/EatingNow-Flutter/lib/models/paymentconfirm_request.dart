import 'OrderRequest.dart';

class PaymentTransaction {
  String Vnp_Amount;
  String Vnp_BankCode;
  String Vnp_BankTranNo;
  String Vnp_CardType;
  String Vnp_OrderInfo;
  String Vnp_PayDate;
  String Vnp_ResponseCode;
  String Vnp_SecureHash;
  String Vnp_TmnCode;
  String Vnp_TransactionNo;
  String Vnp_TransactionStatus;
  String Vnp_TxnRef;
  OrderRequest requestOrder;

  PaymentTransaction({
    required this.Vnp_Amount,
    required this.Vnp_BankCode,
    required this.Vnp_BankTranNo,
    required this.Vnp_CardType,
    required this.Vnp_OrderInfo,
    required this.Vnp_PayDate,
    required this.Vnp_ResponseCode,
    required this.Vnp_SecureHash,
    required this.Vnp_TmnCode,
    required this.Vnp_TransactionNo,
    required this.Vnp_TransactionStatus,
    required this.Vnp_TxnRef,
    required this.requestOrder,
  });

  factory PaymentTransaction.fromJson(Map<String, dynamic> json) {
    return PaymentTransaction(
      Vnp_Amount: json['vnp_Amount'],
      Vnp_BankCode: json['vnp_BankCode'],
      Vnp_BankTranNo: json['vnp_BankTranNo'],
      Vnp_CardType: json['vnp_CardType'],
      Vnp_OrderInfo: json['vnp_OrderInfo'],
      Vnp_PayDate: json['vnp_PayDate'],
      Vnp_ResponseCode: json['vnp_ResponseCode'],
      Vnp_SecureHash: json['vnp_SecureHash'],
      Vnp_TmnCode: json['vnp_TmnCode'],
      Vnp_TransactionNo: json['vnp_TransactionNo'],
      Vnp_TransactionStatus: json['vnp_TransactionStatus'],
      Vnp_TxnRef: json['vnp_TxnRef'],
      requestOrder: OrderRequest.fromJson(json['requestOrder']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'vnp_Amount': Vnp_Amount,
      'vnp_BankCode': Vnp_BankCode,
      'vnp_BankTranNo': Vnp_BankTranNo,
      'vnp_CardType': Vnp_CardType,
      'vnp_OrderInfo': Vnp_OrderInfo,
      'vnp_PayDate': Vnp_PayDate,
      'vnp_ResponseCode': Vnp_ResponseCode,
      'vnp_SecureHash': Vnp_SecureHash,
      'vnp_TmnCode': Vnp_TmnCode,
      'vnp_TransactionNo': Vnp_TransactionNo,
      'vnp_TransactionStatus': Vnp_TransactionStatus,
      'vnp_TxnRef': Vnp_TxnRef,
      'requestOrder': requestOrder.toJson(),
    };
  }
}
