import 'package:fam/storage/cartstorage.dart';

class OrderRequest {
  int? addressId;
  String? completeName;
  String? customerId;
  String? customerName;
  bool? defaut;
  int? districtId;
  String? districtName;
  String? email;
  String? formatAddress;
  int? intoMoney;
  double? latitude;
  double? longitude;
  String? nameAddress;
  List<CartItem>? orderLine;
  String? payment;
  String? phone;
  String? phoneCustomer;
  int? provinceId;
  String? provinceName;
  String? recipientName;
  String? recipientPhone;
  String? tokenApp;
  String? tokenWeb;
  int? totalAmt;
  int? transportFee;
  int? userId;
  int? wardId;
  String? wardName;

  OrderRequest(
      {this.addressId,
        this.completeName,
        this.customerId,
        this.customerName,
        this.defaut,
        this.districtId,
        this.districtName,
        this.email,
        this.formatAddress,
        this.intoMoney,
        this.latitude,
        this.longitude,
        this.nameAddress,
        this.orderLine,
        this.payment,
        this.phone,
        this.phoneCustomer,
        this.provinceId,
        this.provinceName,
        this.recipientName,
        this.recipientPhone,
        this.tokenApp,
        this.tokenWeb,
        this.totalAmt,
        this.transportFee,
        this.userId,
        this.wardId,
        this.wardName});

  OrderRequest.fromJson(Map<String, dynamic> json) {
    addressId = json['AddressId'];
    completeName = json['CompleteName'];
    customerId = json['CustomerId'];
    customerName = json['CustomerName'];
    defaut = json['Defaut'];
    districtId = json['DistrictId'];
    districtName = json['DistrictName'];
    email = json['Email'];
    formatAddress = json['Format_Address'];
    intoMoney = json['IntoMoney'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
    nameAddress = json['Name_Address'];
    if (json['OrderLine'] != null) {
      orderLine = <CartItem>[];
      orderLine = json['OrderLine'].map((v) {
        return CartItem(
          foodListId: v['foodListId'], // Thay 'foodListId' bằng key thích hợp trong JSON
          categoryId: v['CategoryId'],
          foodName: v['foodName'],
          price: v['price'],
          qty: v['qty'],
          uploadImage: v['uploadImage'],
          description: v['description'], descriptionBuy: v['descriptionBuy'],
        );
      }).toList();
    }

    payment = json['Payment'];
    phone = json['Phone'];
    phoneCustomer = json['PhoneCustomer'];
    provinceId = json['ProvinceId'];
    provinceName = json['ProvinceName'];
    recipientName = json['RecipientName'];
    recipientPhone = json['RecipientPhone'];
    tokenApp = json['TokenApp'];
    tokenWeb = json['TokenWeb'];
    totalAmt = json['TotalAmt'];
    transportFee = json['TransportFee'];
    userId = json['UserId'];
    wardId = json['WardId'];
    wardName = json['WardName'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['AddressId'] = this.addressId;
    data['CompleteName'] = this.completeName;
    data['CustomerId'] = this.customerId;
    data['CustomerName'] = this.customerName;
    data['Defaut'] = this.defaut;
    data['DistrictId'] = this.districtId;
    data['DistrictName'] = this.districtName;
    data['Email'] = this.email;
    data['Format_Address'] = this.formatAddress;
    data['IntoMoney'] = this.intoMoney;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['Name_Address'] = this.nameAddress;
    if (this.orderLine != null) {
      data['OrderLine'] = this.orderLine!.map((v) => v.toJsonSentServer()).toList();
    }
    data['Payment'] = this.payment;
    data['Phone'] = this.phone;
    data['PhoneCustomer'] = this.phoneCustomer;
    data['ProvinceId'] = this.provinceId;
    data['ProvinceName'] = this.provinceName;
    data['RecipientName'] = this.recipientName;
    data['RecipientPhone'] = this.recipientPhone;
    data['TokenApp'] = this.tokenApp;
    data['TokenWeb'] = this.tokenWeb;
    data['TotalAmt'] = this.totalAmt;
    data['TransportFee'] = this.transportFee;
    data['UserId'] = this.userId;
    data['WardId'] = this.wardId;
    data['WardName'] = this.wardName;
    print(data);
    return data;
  }
}