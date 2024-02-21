class UserAccountModel {
  bool? success;
  String? message;
  Null? customData;
  Null? item;
  List<Data>? data;
  int? dataCount;

  UserAccountModel(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  UserAccountModel.fromJson(Map<String, dynamic> json) {
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
  String? customerId;
  String? completeName;
  int? provinceId;
  int? districtId;
  dynamic? provinceName;
  dynamic? districtName;
  dynamic? wardName;
  int? wardId;
  String? phone;
  String? email;
  dynamic? address;
  bool? status;
  dynamic? tokenWeb;
  dynamic? tokenApp;
  String? imageProfile;

  Data(
      {this.customerId,
        this.completeName,
        this.provinceId,
        this.districtId,
        this.provinceName,
        this.districtName,
        this.wardName,
        this.wardId,
        this.phone,
        this.email,
        this.address,
        this.status,
        this.tokenWeb,
        this.tokenApp,
        this.imageProfile});

  Data.fromJson(Map<String, dynamic> json) {
    customerId = json['CustomerId'];
    completeName = json['CompleteName'];
    provinceId = json['ProvinceId'];
    districtId = json['DistrictId'];
    provinceName = json['ProvinceName'];
    districtName = json['DistrictName'];
    wardName = json['WardName'];
    wardId = json['WardId'];
    phone = json['Phone'];
    email = json['Email'];
    address = json['Address'];
    status = json['Status'];
    tokenWeb = json['TokenWeb'];
    tokenApp = json['TokenApp'];
    imageProfile = json['ImageProfile'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['CustomerId'] = this.customerId;
    data['CompleteName'] = this.completeName;
    data['ProvinceId'] = this.provinceId;
    data['DistrictId'] = this.districtId;
    data['ProvinceName'] = this.provinceName;
    data['DistrictName'] = this.districtName;
    data['WardName'] = this.wardName;
    data['WardId'] = this.wardId;
    data['Phone'] = this.phone;
    data['Email'] = this.email;
    data['Address'] = this.address;
    data['Status'] = this.status;
    data['TokenWeb'] = this.tokenWeb;
    data['TokenApp'] = this.tokenApp;
    data['ImageProfile'] = this.imageProfile;
    return data;
  }
}
