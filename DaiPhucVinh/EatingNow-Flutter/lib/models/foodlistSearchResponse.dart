class FoodListSearchResponse {
  bool? success;
  String? message;
  Null? customData;
  Null? item;
  List<Data>? data;
  int? dataCount;

  FoodListSearchResponse(
      {this.success,
        this.message,
        this.customData,
        this.item,
        this.data,
        this.dataCount});

  FoodListSearchResponse.fromJson(Map<String, dynamic> json) {
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
  StoreinFo? storeinFo;
  List<FoodList>? foodList;

  Data({this.storeinFo, this.foodList});

  Data.fromJson(Map<String, dynamic> json) {
    storeinFo = json['storeinFo'] != null
        ? new StoreinFo.fromJson(json['storeinFo'])
        : null;
    if (json['foodList'] != null) {
      foodList = <FoodList>[];
      json['foodList'].forEach((v) {
        foodList!.add(new FoodList.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.storeinFo != null) {
      data['storeinFo'] = this.storeinFo!.toJson();
    }
    if (this.foodList != null) {
      data['foodList'] = this.foodList!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class StoreinFo {
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
  int? distance;
  int? time;

  StoreinFo(
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
        this.time});

  StoreinFo.fromJson(Map<String, dynamic> json) {
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
    return data;
  }
}

class FoodList {
  int? foodListId;
  String? category;
  int? categoryId;
  String? foodName;
  Null? storeName;
  int? price;
  int? qty;
  String? uploadImage;
  String? description;
  int? userId;
  bool? status;
  int? hint;
  bool? isNew;
  bool? isNoiBat;
  int? quantitySupplied;
  String? expiryDate;
  bool? qtycontrolled;
  bool? qtySuppliedcontrolled;
  int? latitude;
  int? longitude;

  FoodList(
      {this.foodListId,
        this.category,
        this.categoryId,
        this.foodName,
        this.storeName,
        this.price,
        this.qty,
        this.uploadImage,
        this.description,
        this.userId,
        this.status,
        this.hint,
        this.isNew,
        this.isNoiBat,
        this.quantitySupplied,
        this.expiryDate,
        this.qtycontrolled,
        this.qtySuppliedcontrolled,
        this.latitude,
        this.longitude});

  FoodList.fromJson(Map<String, dynamic> json) {
    foodListId = json['FoodListId'];
    category = json['Category'];
    categoryId = json['CategoryId'];
    foodName = json['FoodName'];
    storeName = json['storeName'];
    price = json['Price'];
    qty = json['qty'];
    uploadImage = json['UploadImage'];
    description = json['Description'];
    userId = json['UserId'];
    status = json['Status'];
    hint = json['Hint'];
    isNew = json['IsNew'];
    isNoiBat = json['IsNoiBat'];
    quantitySupplied = json['QuantitySupplied'];
    expiryDate = json['ExpiryDate'];
    qtycontrolled = json['Qtycontrolled'];
    qtySuppliedcontrolled = json['QtySuppliedcontrolled'];
    latitude = json['Latitude'];
    longitude = json['Longitude'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['FoodListId'] = this.foodListId;
    data['Category'] = this.category;
    data['CategoryId'] = this.categoryId;
    data['FoodName'] = this.foodName;
    data['storeName'] = this.storeName;
    data['Price'] = this.price;
    data['qty'] = this.qty;
    data['UploadImage'] = this.uploadImage;
    data['Description'] = this.description;
    data['UserId'] = this.userId;
    data['Status'] = this.status;
    data['Hint'] = this.hint;
    data['IsNew'] = this.isNew;
    data['IsNoiBat'] = this.isNoiBat;
    data['QuantitySupplied'] = this.quantitySupplied;
    data['ExpiryDate'] = this.expiryDate;
    data['Qtycontrolled'] = this.qtycontrolled;
    data['QtySuppliedcontrolled'] = this.qtySuppliedcontrolled;
    data['Latitude'] = this.latitude;
    data['Longitude'] = this.longitude;
    return data;
  }
}
