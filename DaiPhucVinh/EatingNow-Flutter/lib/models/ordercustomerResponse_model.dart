class OrderCustomerResponse {
  bool? success;
  String? message;
  dynamic? customData;
  dynamic? item;
  List<Data>? data;
  int? dataCount;

  OrderCustomerResponse(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  OrderCustomerResponse.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'];
    if (json['Data'] != null) {
      data = <Data>[];
      json['Data'].forEach((v) {
        data!.add(new Data.fromJson(v));
      });
    }
    dataCount = json['DataCount'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Success'] = this.success;
    data['Message'] = this.message;
    data['CustomData'] = this.customData;
    data['Item'] = this.item;
    if (this.data != null) {
      data['Data'] = this.data!.map((v) => v.toJson()).toList();
    }
    data['DataCount'] = this.dataCount;
    return data;
  }
}

class Data {
  String? orderHeaderId;
  String? creationDate;
  String? customerName;
  String? phone;
  dynamic? address;
  String? customerId;
  String? tokenWeb;
  dynamic? tokenApp;
  double? totalAmt;
  double? transportFee;
  double? intoMoney;
  int? userId;
  String? storeName;
  bool? status;
  dynamic? email;
  String? recipientName;
  String? recipientPhone;
  String? formatAddress;
  String? nameAddress;
  double? latitude;
  double? longitude;
  int? paymentStatusID;

  Data(
      {this.orderHeaderId,
        this.creationDate,
        this.customerName,
        this.phone,
        this.address,
        this.customerId,
        this.tokenWeb,
        this.tokenApp,
        this.totalAmt,
        this.transportFee,
        this.intoMoney,
        this.userId,
        this.storeName,
        this.status,
        this.email,
        this.recipientName,
        this.recipientPhone,
        this.formatAddress,
        this.nameAddress,
        this.latitude,
        this.longitude,
        this.paymentStatusID});

  Data.fromJson(Map<String, dynamic> json) {
    orderHeaderId = json['OrderHeaderId'];
    creationDate = json['CreationDate'];
    customerName = json['CustomerName'];
    phone = json['Phone'];
    address = json['Address'];
    customerId = json['CustomerId'];
    tokenWeb = json['TokenWeb'];
    tokenApp = json['TokenApp'];
    totalAmt = json['TotalAmt'];
    transportFee = json['TransportFee'];
    intoMoney = json['IntoMoney'];
    userId = json['UserId'];
    storeName = json['StoreName'];
    status = json['Status'];
    email = json['Email'];
    recipientName = json['RecipientName'];
    recipientPhone = json['RecipientPhone'];
    formatAddress = json['FormatAddress'];
    nameAddress = json['NameAddress'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
    paymentStatusID = json['PaymentStatusID'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['OrderHeaderId'] = this.orderHeaderId;
    data['CreationDate'] = this.creationDate;
    data['CustomerName'] = this.customerName;
    data['Phone'] = this.phone;
    data['Address'] = this.address;
    data['CustomerId'] = this.customerId;
    data['TokenWeb'] = this.tokenWeb;
    data['TokenApp'] = this.tokenApp;
    data['TotalAmt'] = this.totalAmt;
    data['TransportFee'] = this.transportFee;
    data['IntoMoney'] = this.intoMoney;
    data['UserId'] = this.userId;
    data['StoreName'] = this.storeName;
    data['Status'] = this.status;
    data['Email'] = this.email;
    data['RecipientName'] = this.recipientName;
    data['RecipientPhone'] = this.recipientPhone;
    data['FormatAddress'] = this.formatAddress;
    data['NameAddress'] = this.nameAddress;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['PaymentStatusID'] = this.paymentStatusID;
    return data;
  }
}
