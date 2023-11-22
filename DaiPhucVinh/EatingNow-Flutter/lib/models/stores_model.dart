class StoreModel {
  bool? success;
  Null? message;
  Null? customData;
  Null? item;
  List<Data>? data;
  int? dataCount;
  late List<StoreModel>? _Store;
  List<StoreModel>? get Store => _Store;

  StoreModel(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  StoreModel.fromJson(Map<String, dynamic> json) {
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
  int? userId;
  String? absoluteImage;
  String? fullName;
  String? description;
  String? openTime;
  String? province;
  String? cuisine;
  String? email;
  String? address;
  String? ownerName;
  String? phone;
  double? latitude;
  double? longitude;
  String? status;
  double? distance;
  double? time;
  Data(
      {this.userId,
        this.absoluteImage,
        this.fullName,
        this.description,
        this.openTime,
        this.province,
        this.cuisine,
        this.email,
        this.address,
        this.ownerName,
        this.phone,
        this.latitude,
        this.longitude,
        this.status,
        this.distance,
        this.time});

  Data.fromJson(Map<String, dynamic> json) {
    userId = json['UserId'];
    absoluteImage = json['AbsoluteImage'];
    fullName = json['FullName'];
    description = json['Description'];
    openTime = json['OpenTime'];
    province = json['Province'];
    cuisine = json['Cuisine'];
    email = json['Email'];
    address = json['Address'];
    ownerName = json['OwnerName'];
    phone = json['Phone'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
    status = json['Status'];
    distance = json['Distance'];
    time = json['Time'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['UserId'] = this.userId;
    data['AbsoluteImage'] = this.absoluteImage;
    data['FullName'] = this.fullName;
    data['Description'] = this.description;
    data['OpenTime'] = this.openTime;
    data['Province'] = this.province;
    data['Cuisine'] = this.cuisine;
    data['Email'] = this.email;
    data['Address'] = this.address;
    data['OwnerName'] = this.ownerName;
    data['Phone'] = this.phone;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['Status'] = this.status;
    data['Distance'] = this.distance;
    data['Time'] = this.time;
    return data;
  }
}