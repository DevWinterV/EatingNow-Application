import 'package:fam/models/storenearUser.dart';

class StoreDataResponse {
  bool? success;
  Null? message;
  Null? customData;
  Item? item;
  Null? data;
  int? dataCount;

  StoreDataResponse(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  StoreDataResponse.fromJson(Map<String, dynamic> json) {
    success = json['Success'];
    message = json['Message'];
    customData = json['CustomData'];
    item = json['Item'] != null ? new Item.fromJson(json['Item']) : null;
    data = json['Data'];
    dataCount = json['DataCount'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['Success'] = this.success;
    data['Message'] = this.message;
    data['CustomData'] = this.customData;
    if (this.item != null) {
      data['Item'] = this.item!.toJson();
    }
    data['Data'] = this.data;
    data['DataCount'] = this.dataCount;
    return data;
  }
}

class Item {
  int? userId;
  String? absoluteImage;
  String? fullName;
  String? description;
  String? openTime;
  String? province;
  int? provinceId;
  String? cuisine;
  int? cuisineId;
  String? email;
  String? address;
  String? ownerName;
  String? phone;
  double? latitude;
  double? longitude;
  String? status;
  double? distance;
  double? time;
  double? similarity;

  Item(
      {this.userId,
        this.absoluteImage,
        this.fullName,
        this.description,
        this.openTime,
        this.province,
        this.provinceId,
        this.cuisine,
        this.cuisineId,
        this.email,
        this.address,
        this.ownerName,
        this.phone,
        this.latitude,
        this.longitude,
        this.status,
        this.distance,
        this.time,
        this.similarity});

  Item.fromJson(Map<String, dynamic> json) {
    userId = json['UserId'];
    absoluteImage = json['AbsoluteImage'];
    fullName = json['FullName'];
    description = json['Description'];
    openTime = json['OpenTime'];
    province = json['Province'];
    provinceId = json['ProvinceId'];
    cuisine = json['Cuisine'];
    cuisineId = json['CuisineId'];
    email = json['Email'];
    address = json['Address'];
    ownerName = json['OwnerName'];
    phone = json['Phone'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
    status = json['Status'];
    distance = json['Distance'];
    time = json['Time'];
    similarity = json['similarity'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['UserId'] = this.userId;
    data['AbsoluteImage'] = this.absoluteImage;
    data['FullName'] = this.fullName;
    data['Description'] = this.description;
    data['OpenTime'] = this.openTime;
    data['Province'] = this.province;
    data['ProvinceId'] = this.provinceId;
    data['Cuisine'] = this.cuisine;
    data['CuisineId'] = this.cuisineId;
    data['Email'] = this.email;
    data['Address'] = this.address;
    data['OwnerName'] = this.ownerName;
    data['Phone'] = this.phone;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    data['Status'] = this.status;
    data['Distance'] = this.distance;
    data['Time'] = this.time;
    data['similarity'] = this.similarity;
    return data;
  }
  DataStoreNearUserModel toDataStoreNearUserModel() {
    return DataStoreNearUserModel(
      userId: userId,
      absoluteImage: absoluteImage,
      fullName: fullName,
      description: description,
      openTime: openTime,
      province: province,
      provinceId: provinceId,
      cuisine: cuisine,
      cuisineId: cuisineId,
      email: email,
      address: address,
      ownerName: ownerName,
      phone: phone,
      latitude: latitude,
      longitude: longitude,
      status: status,
      distance: distance?.toDouble(), // Convert int? to double?
      time: time?.toDouble(), // Convert int? to double?
    );
  }
}
